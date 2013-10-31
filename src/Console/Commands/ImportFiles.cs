using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RbcConsole.Helpers;
using RbcTools.Library;
using RbcTools.Library.Database;

namespace RbcConsole.Commands
{
	public class ImportFiles : CommandBase
	{
		public ImportFiles()
		{
			base.Slug = "import-files";
			base.Description = "Import S82 files (PDF) into the RBC SharePoint database";
			base.IsDatabaseCommand = true;
		}
		
		#region Fields
		
		private Volunteer CurrentVolunteer;
		private S82Reader CurrentReader;
		private Process OpenFileProcess;
		private bool skipFile = false;
		private bool skipCommand = false;
		private int currentCongregationID = 0;
		
		#endregion
		
		public override void Run()
		{
			this.SelectCongregation();
			
			if(this.skipCommand == false)
			{
				this.RunImportFiles();
			}
			else
				ConsoleX.WriteLine("Import Files Skipped", ConsoleColor.Red);
			
			// Reset for next Run()
			this.skipCommand = false;
			this.currentCongregationID = 0;
		}

		private void SelectCongregation()
		{
			ConsoleX.WriteLine("First, please enter the ID of the Congregation you are about to import.");
			do
			{
				var id = ConsoleX.WriteIntegerQuery("Enter Congregation ID:", allowSkip: true);
				if(id == int.MinValue)
				{
					this.skipCommand = true;
					this.currentCongregationID = id;
				}
				else
				{
					var table = Congregations.GetById(id);
					if(table.Rows.Count == 1)
					{
						ConsoleX.WriteLine("Selected congregation:", false);
						ConsoleX.WriteDataTable(table);
						if (ConsoleX.WriteBooleanQuery("Is this correct?"))
							this.currentCongregationID = id;
						else
							ConsoleX.WriteLine("No, okay. Please try again.");
					}
					else
					{
						ConsoleX.WriteLine("No Congregation Found. Please try again.", ConsoleColor.Red);
					}
				}
			}
			while(this.currentCongregationID == 0);
		}
		
		private void RunImportFiles()
		{
			var fileNames = ImportFiles.GetFiles(base.ConsoleX);
			
			if(fileNames != null)
			{
				foreach (string fileName in fileNames)
				{
					this.OpenS82Reader(fileName);
				}
				ConsoleX.WriteLine("All files completed!");
			}
			else
				ConsoleX.WriteWarning("No files selected!");
		}
		
		private void OpenS82Reader(string fileName)
		{
			ConsoleX.WriteLine(string.Format("Reading '{0}' file...", fileName), ConsoleColor.Green);
			
			this.skipFile = false;
			
			this.CurrentReader = new S82Reader(fileName);
			
			if(this.CurrentReader.IsReadable)
			{
				this.ProcessVolunteer();
			}
			else
			{
				ConsoleX.WriteWarning("File is not readable. Perhaps it is not in the correct format.");
				ConsoleX.WriteLine("Press any key to continue.");
				Console.ReadKey();
				this.skipFile = true;
			}
			
			this.CurrentReader = null;
			
			if(this.skipFile)
				ConsoleX.WriteLine(string.Format("Skipping '{0}'", fileName), ConsoleColor.Red);
			else
				ConsoleX.WriteLine(string.Format("Finished '{0}'", fileName), ConsoleColor.Green);
			
			ConsoleX.WriteHorizontalRule();
			
		}
		
		private void ProcessVolunteer()
		{
			this.CurrentVolunteer = new Volunteer();
			this.CurrentVolunteer.CongregationID = this.currentCongregationID;
			
			ConsoleX.WriteLine("I'll open the file for you to check the data as we go.");
			
			this.OpenFileInNewProcess();
			
			#region Step #1 Get name and gender
			
			ConsoleX.WriteLine("Step #1 Get name and gender", ConsoleColor.Green);
			
			this.Step1_NameAndGender();
			
			#endregion
			
			#region Step #2 Search for existing records (use existing or create new)
			
			ConsoleX.WriteLine("Step #2 Search for existing records", ConsoleColor.Green);
			
			this.Step2_SelectRecord();
			
			#endregion
			
			if(!this.skipFile)
			{
				
				#region Step #3 Read the rest of the form
				
				ConsoleX.WriteLine("Step #3 Read the rest of the file", ConsoleColor.Green);
				
				this.Step3_ReadFileData();
				
				#endregion
				
				#region Step #4 Display details and confirm
				
				ConsoleX.WriteLine("Step #4 Display collected details and confirm save", ConsoleColor.Green);
				
				this.Step4_DisplayDetails();
				
				this.Step4_Save();
				
				#endregion
				
			}
			
			this.CloseFileIfProcessOpen();
			
			this.CurrentVolunteer = null;
		}
		
		#region Step Methods
		
		private void Step1_NameAndGender()
		{
			
			#region Gender
			
			var maleInput = this.CurrentReader.GetCheckBoxValue("Check Box5");
			var femaleInput = this.CurrentReader.GetCheckBoxValue("Check Box6");
			
			if(maleInput == femaleInput)
			{
				this.INeedYourHelp("Gender");
				
				if(ConsoleX.WriteBooleanQuery("Is the volunteer male?"))
					this.CurrentVolunteer.Gender = GenderKind.Male;
				else
					this.CurrentVolunteer.Gender = GenderKind.Female;
			}
			else
			{
				this.CurrentVolunteer.Gender = maleInput ? GenderKind.Male : GenderKind.Female;
			}
			
			ConsoleX.WriteLine("Volunteer's Gender: " + this.CurrentVolunteer.Gender.ToString());
			
			#endregion
			
			#region Names
			
			// Get Surname, FirstName and MiddleName
			
			string pdfValue = this.CurrentReader["Text2"];
			
			// Remove any periods or commas (.,)
			pdfValue = pdfValue.Replace(".", "").Replace(",", "");
			
			string lastName, firstName, middleNames = "";
			
			var names = pdfValue.Split(' ');
			if(names.Length == 3)
			{
				lastName = names[0];
				firstName = names[1];
				middleNames = names[2];
			}
			else if(names.Length == 2)
			{
				lastName = names[0];
				firstName = names[1];
			}
			else
			{
				this.INeedYourHelp("Name");
				
				lastName = ConsoleX.WriteQuery("Please can you tell me their 'Last Name'?");
				middleNames = ConsoleX.WriteQuery("Please can you tell me their 'Middles Names'? (Leave blank if they don't have any)");
				firstName = ConsoleX.WriteQuery("Please can you tell me their 'First Name'?");
			}
			
			this.CurrentVolunteer.LastName = lastName;
			this.CurrentVolunteer.MiddleNames = middleNames;
			this.CurrentVolunteer.FirstName = firstName;
			
			ConsoleX.WriteLine(string.Format("Volunteer's Name: {0} {1}", this.CurrentVolunteer.FirstName, this.CurrentVolunteer.LastName));
			
			#endregion
			
		}
		
		private void Step2_SelectRecord()
		{
			
			ConsoleX.WriteLine(string.Format("Looking up '{0} {1}'...", this.CurrentVolunteer.FirstName, this.CurrentVolunteer.LastName));
			
			bool matchesFound = VolunteerLookup.TrySearchForNames(this.CurrentVolunteer.FirstName, this.CurrentVolunteer.LastName, ConsoleX);
			
			if(matchesFound)
			{
				ConsoleX.WriteLine("Step #2.1 Select an existing record to update", ConsoleColor.Green);
				ConsoleX.WriteLine("Do you want to update an existing record?");
				
				var input = string.Empty;
				
				do
				{
					input = ConsoleX.WriteQuery("Enter a valid ID, or press ENTER to skip:");
					
					int possibleID;
					if(int.TryParse(input, out possibleID))
					{
						var existingVolunteer = Volunteers.GetByID(possibleID);
						if(existingVolunteer != null)
						{
							ConsoleX.WriteLine(string.Format("You have selected {0} - {1} {2}.",
							                                 existingVolunteer.ID,
							                                 existingVolunteer.FirstName,
							                                 existingVolunteer.LastName));
							
							var confirm = ConsoleX.WriteQuery("Is this correct? Enter 'yes' to confirm, ENTER to try again.").ToLower();
							
							if(confirm == "yes")
							{
								input = string.Empty; // So that we can leave loop.
								this.CurrentVolunteer.ID = existingVolunteer.ID;
								ConsoleX.WriteLine(string.Format("Using record ID {0}", this.CurrentVolunteer.ID));
							}
						}
						else
						{
							ConsoleX.WriteWarning("ID Incorrect, please try again.");
						}
					}
				} while(!string.IsNullOrEmpty(input));
			}
			
			if(this.CurrentVolunteer.ID == 0)
			{
				ConsoleX.WriteLine("Step #2.2 Confirm this is a new record", ConsoleColor.Green);
				ConsoleX.WriteLine("Continuing will create a NEW record.");
				var confirm = ConsoleX.WriteQuery("Is this correct? Enter 'yes' to confirm, or anything else to skip this file.").ToLower();
				if(confirm != "yes")
					this.skipFile = true;
			}
		}
		
		private void Step3_ReadFileData()
		{
			
			this.Step3_ApplicationKind();
			this.Step3_FormsOfService();
			this.Step3_Dates();

			// Postal Address
			this.CurrentVolunteer.Address = this.CurrentReader["Text5"];

			// Email Address
			this.CurrentVolunteer.EmailAddress = this.CurrentReader["Text6"];

			this.Step3_TelephoneNumbers();

			this.Step3_Privileges();

			// Name Of Mate
			this.CurrentVolunteer.NameOfMate = this.CurrentReader["Text10"];

			this.Step3_WorkBackground();
			
			this.Step3_EmergencyContactNameAndRelationship();
			
			this.Step3_EmergencyContactTelAndAddress();
			
		}
		
		private void Step3_ApplicationKind()
		{
			var newApplicationInput = this.CurrentReader.GetCheckBoxValue("Check Box1");
			var updateApplicationInput = this.CurrentReader.GetCheckBoxValue("Check Box2");
			
			bool isUpdate = false;
			
			if(!newApplicationInput && !updateApplicationInput)
			{
				if(this.CurrentVolunteer.ID != 0)
					isUpdate = true;
			}
			else if(updateApplicationInput)
			{
				isUpdate = true;
			}
			
			if(isUpdate)
				this.CurrentVolunteer.ApplicationKind = ApplicationKind.UpdateOfPersonalData;
			else
				this.CurrentVolunteer.ApplicationKind = ApplicationKind.NewApplication;
			
		}
		
		private void Step3_FormsOfService()
		{
			var constructionInput = this.CurrentReader.GetCheckBoxValue("Check Box3");
			var disasterInput = this.CurrentReader.GetCheckBoxValue("Check Box4");
			
			if(constructionInput)
				this.CurrentVolunteer.FormsOfService |= FormOfServiceKinds.HallConstruction;
			
			if(disasterInput)
				this.CurrentVolunteer.FormsOfService |= FormOfServiceKinds.DisasterRelief;
		}
		
		private void Step3_Dates()
		{
			var birthDate = this.CurrentReader.GetDateTimeValue("Text3");
			var baptismDate = this.CurrentReader.GetDateTimeValue("Text4");
			
			if(birthDate == DateTime.MinValue)
			{
				this.INeedYourHelp("Date of Birth");
				
				birthDate = ConsoleX.WriteDateTimeQuery("Please can you tell me their Date of Birth?");
			}
			
			if(baptismDate == DateTime.MinValue)
			{
				this.INeedYourHelp("Date of Baptism");
				
				baptismDate = ConsoleX.WriteDateTimeQuery("Please can you tell me their Date of Baptism?");
			}
			
			this.CurrentVolunteer.DateOfBirth = birthDate;
			this.CurrentVolunteer.DateOfBaptism = baptismDate;
		}
		
		private void Step3_TelephoneNumbers()
		{
			var home = this.CurrentReader["Text7"];
			var work = this.CurrentReader["Text8"];
			var mobile = this.CurrentReader["Text9"];
			
			// TODO Better telephone validation
			
			this.CurrentVolunteer.PhoneNumberHome = home;
			this.CurrentVolunteer.PhoneNumberWork = work;
			this.CurrentVolunteer.PhoneNumberMobile = mobile;
		}
		
		private void Step3_Privileges()
		{
			
			var elder = this.CurrentReader.GetCheckBoxValue("Check Box7.0");
			var servant = this.CurrentReader.GetCheckBoxValue("Check Box7.1");
			var pioneer = this.CurrentReader.GetCheckBoxValue("Check Box7.2");
			
			if(elder)
				this.CurrentVolunteer.CongregationPrivileges |= CongregationPrivilegeKinds.Elder;
			else if(servant)
				this.CurrentVolunteer.CongregationPrivileges |= CongregationPrivilegeKinds.MinisterialServant;
			
			if(pioneer)
				this.CurrentVolunteer.RegularPioneer = true;
			
		}
		
		private void Step3_WorkBackground()
		{
			this.CurrentVolunteer.WorkBackgroundList = new List<WorkBackground>();
			
			Action<int, string, string, string> createBackground = delegate(int row, string field1, string field2, string field3)
			{
				var bg = new WorkBackground();
				this.CurrentVolunteer.WorkBackgroundList.Add(bg);
				
				bg.TradeOrProfession = this.CurrentReader[field1];
				bg.TypeOfExprience = this.CurrentReader[field2];
				
				string yearsStr = this.CurrentReader[field3];
				if(!string.IsNullOrEmpty(yearsStr))
				{
					int years;
					if(int.TryParse(yearsStr, out years))
					{
						bg.Years = years;
					}
					else
					{
						this.INeedYourHelp(string.Format("Work Background {0} - Years", row));
						
						ConsoleX.WriteLine(string.Format("The years experience fields says '{0}', but I don't understand this.", yearsStr));
						
						bg.Years = ConsoleX.WriteIntegerQuery("Please can you tell me the number of years experience?");
					}
				}
			};
			
			createBackground(1, "Text11.0.0", "Text11.0.1", "Text12.0.0");
			createBackground(2, "Text11.1.0", "Text11.1.1", "Text12.0.1");
			createBackground(3, "Text11.2.0", "Text11.2.1", "Text12.0.2");
			createBackground(4, "Text11.3.0", "Text11.3.1", "Text12.0.3");
		}
		
		private void Step3_EmergencyContactNameAndRelationship()
		{
			var text13 = this.CurrentReader["Text13"];
			if(!string.IsNullOrEmpty(text13))
			{
				var name = "";
				var relationship = "";
				
				// Take a guess (assume last word is relationship);
				var parts = text13.Split(' ');
				if(parts.Length == 0)
				{
					// only one word found - assume name
					name = text13;
				}
				else
				{
					relationship = parts[parts.Length-1];
					name = text13.Replace(relationship, "").Trim();
				}
				
				// Ask if correct, and if not get help until it is correct.
				while(!Step3_EmergencyContactNameAndRelationship_Check(name, relationship))
				{
					name = ConsoleX.WriteClipboardQuery("Emergency Contact Name");
					relationship = text13.Replace(name, "").Trim();
				}
				
				this.CurrentVolunteer.EmergencyContactName = name;
				this.CurrentVolunteer.EmergencyContactRelationship = relationship;
			}
		}
		
		private bool Step3_EmergencyContactNameAndRelationship_Check(string name, string relationship)
		{
			return ConsoleX.WriteFieldCheck("Emergency Contact Name", name, "Emergency Contact Relationship", relationship);
		}
		
		private void Step3_EmergencyContactTelAndAddress()
		{
			string text14 = this.CurrentReader["Text14"];
			if(!string.IsNullOrEmpty(text14))
			{
				string phone = "";
				string address = "";
				
				// Try to find telephone number(s)
				string lastMatch = "";
				foreach(Match match in Regex.Matches(text14, @"\(?0( *\d\)?){9,10}"))
				{
					if(!string.IsNullOrEmpty(phone)) phone += " or ";
					phone += match.Value;
					lastMatch = match.Value;
				}
				
				// Find address by using everything after last found telephone number
				var startIndex = text14.LastIndexOf(lastMatch) + lastMatch.Length;
				address = text14.Substring(startIndex).Trim();
				
				// Ask if correct, and if not get help until it is correct.
				while(!Step3_EmergencyContactTelAndAddress_Check(phone, address))
				{
					address = ConsoleX.WriteClipboardQuery("Emergency Contact Address");
					phone = text14.Replace(address, "").Trim();
				}
				
				this.CurrentVolunteer.EmergencyContactPhoneNumber = phone;
				this.CurrentVolunteer.EmergencyContactAddress = address;
			}
		}
		
		private bool Step3_EmergencyContactTelAndAddress_Check(string phone, string address)
		{
			return ConsoleX.WriteFieldCheck("Emergency Contact Phone Number(s)", phone, "Emergency Contact Address", address);
		}
		
		private void Step4_DisplayDetails()
		{
			ConsoleX.WriteLine("The following details were collected:", ConsoleColor.Green);
			
			// Application Kind
			ConsoleX.WriteLine(string.Format("Application Kind = {0}", this.CurrentVolunteer.ApplicationKind.GetName()));
			
			// Forms of service
			var message = "Forms of service: ";
			
			if(this.CurrentVolunteer.FormsOfService.HasFlag(FormOfServiceKinds.HallConstruction))
				message += " * Hall Construction ";
			
			if(this.CurrentVolunteer.FormsOfService.HasFlag(FormOfServiceKinds.DisasterRelief))
				message += " * Disaster Relief ";
			
			if(this.CurrentVolunteer.FormsOfService == FormOfServiceKinds.NoneSpecified)
				message += " None Specified ";
			
			ConsoleX.WriteLine(message);
			
			// Names and Gender
			
			ConsoleX.WriteLine("First name: " + this.CurrentVolunteer.FirstName);
			ConsoleX.WriteLine("Middle name(s): " + this.CurrentVolunteer.MiddleNames);
			ConsoleX.WriteLine("Last name: " + this.CurrentVolunteer.LastName);
			ConsoleX.WriteLine("Gender: " + this.CurrentVolunteer.Gender.ToString());
			
			// Dates
			ConsoleX.WriteLine("Date of birth: " + this.CurrentVolunteer.DateOfBirth.ToLongDateString());
			ConsoleX.WriteLine("Date of baptism: " + this.CurrentVolunteer.DateOfBaptism.ToLongDateString());
			
			// Postal Address
			ConsoleX.WriteLine("Postal Address: " + this.CurrentVolunteer.Address);
			
			// Email Address
			ConsoleX.WriteLine("Email Address: " + this.CurrentVolunteer.EmailAddress);
			
			// Phone numbers
			ConsoleX.WriteLine("Home Phone: " + this.CurrentVolunteer.PhoneNumberHome);
			ConsoleX.WriteLine("Work Phone: " + this.CurrentVolunteer.PhoneNumberWork);
			ConsoleX.WriteLine("Mobile Phone: " + this.CurrentVolunteer.PhoneNumberMobile);
			
			// Current privileges
			message = "Current Privileges: ";
			
			if(this.CurrentVolunteer.CongregationPrivileges.HasFlag(CongregationPrivilegeKinds.Elder))
			{
				message += " * Elder ";
			}
			else if(this.CurrentVolunteer.CongregationPrivileges.HasFlag(CongregationPrivilegeKinds.MinisterialServant))
			{
				message += " * Ministerial Servant ";
			}
			
			if(this.CurrentVolunteer.RegularPioneer)
			{
				message += " * Regular Pioneer ";
			}
			
			if(!this.CurrentVolunteer.RegularPioneer && this.CurrentVolunteer.CongregationPrivileges == CongregationPrivilegeKinds.NoneSpecified)
				message += " None Specified ";
			
			ConsoleX.WriteLine(message);
			
			// Name of Mate
			ConsoleX.WriteLine("Name of mate: " + this.CurrentVolunteer.NameOfMate);
			
			// Work background
			ConsoleX.WriteLine("Work background:");
			DataTable table = new DataTable();
			table.Columns.Add("Trade or profession");
			table.Columns.Add("Type of experience");
			table.Columns.Add("Years");
			
			Action<int> displayBackground = delegate(int index)
			{
				var row = table.NewRow();
				var bg = this.CurrentVolunteer.WorkBackgroundList[index];
				row[0] = bg.TradeOrProfession;
				row[1] = bg.TypeOfExprience;
				row[2] = bg.Years;
				table.Rows.Add(row);
			};
			
			displayBackground(0);
			displayBackground(1);
			displayBackground(2);
			displayBackground(3);
			
			ConsoleX.WriteDataTable(table);
			
			// Emergency Contact Details
			ConsoleX.WriteLine("Emergency Contact Name: " + this.CurrentVolunteer.EmergencyContactName);
			ConsoleX.WriteLine("Emergency Contact Relationship: " + this.CurrentVolunteer.EmergencyContactRelationship);
			ConsoleX.WriteLine("Emergency Contact Phone Number(s): " + this.CurrentVolunteer.EmergencyContactPhoneNumber);
			ConsoleX.WriteLine("Emergency Contact Address: " + this.CurrentVolunteer.EmergencyContactAddress);
		}
		
		private void Step4_Save()
		{
			if(ConsoleX.WriteBooleanQuery("Shall I save to the database?"))
			{
//				this.CurrentVolunteer.SaveToDatabase();
				ConsoleX.WriteWarning("DISABLED: Please note that the saving functionality is currently disabled in this test");
				
				if(this.CurrentVolunteer.ID == 0)
					ConsoleX.WriteLine("DONE: Inserted a new record to the database!", ConsoleColor.Magenta);
				else
					ConsoleX.WriteLine("DONE: Updated the record in the database!", ConsoleColor.Magenta);
			}
			else
			{
				ConsoleX.WriteWarning("UNSAVED: This record was not saved.");
			}
		}
		
		#endregion
		
		#region Reusable Methods
		
		public static string[] GetFiles(IConsoleX consoleX)
		{
			consoleX.WriteLine("Press any key to select the S82 PDF files...");
			Console.ReadKey();
			
			string[] files = null;
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Multiselect = true;
			dlg.Title = "Choose S82 PDF files";
			dlg.Filter = "PDF Files|*.pdf";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				files = dlg.FileNames;
			}
			return files;
		}
		
		public void INeedYourHelp(string fieldName)
		{
			ConsoleX.WriteWarning(string.Format("I'm having trouble with the field: '{0}'. I need your help. ", fieldName));
		}
		
		public void OpenFileInNewProcess()
		{
			if(this.OpenFileProcess == null)
			{
				this.OpenFileProcess = Process.Start(this.CurrentReader.FilePath);
			}
		}
		
		public void CloseFileIfProcessOpen()
		{
			if(this.OpenFileProcess != null)
			{
				var process = this.OpenFileProcess;
				this.OpenFileProcess = null;
				try
				{
					process.Kill();
				}
				catch (Exception) {}
			}
		}
		
		#endregion
		
	}
}
