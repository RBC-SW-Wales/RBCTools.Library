using System;
using System.Diagnostics;
using System.Windows.Forms;
using RbcVolunteerApplications.Library;
using RbcVolunteerApplications.Library.Database;

namespace RbcVolunteerApplications.Importer.Commands
{
	public class ImportFiles : CommandBase
	{
		public ImportFiles()
		{
			base.Slug = "import-files";
			base.Description = "Import S82 files (PDF) into the RBC SharePoint database";
		}
		
		private Volunteer CurrentVolunteer;
		private S82Reader CurrentReader;
		
		private Process OpenFileProcess;
		
		public override void Run()
		{
			ConsoleX.WriteIntro(base.Description);
			
//			this.ShowFields();
			this.RunImportFiles();
			
			ConsoleX.WriteHorizontalRule();
		}
		
		public void ShowFields()
		{
			ConsoleX.WriteLine("Press any key to select the S82 PDF files...");
			Console.ReadKey(true);
			var fileNames = GetFiles();
			foreach(var fileName in fileNames)
			{
				var reader = new S82Reader(fileName);
				ConsoleX.WriteLine("Reading fields from " + reader.FilePath, ConsoleColor.Green);
				
				foreach(var key in reader.Keys)
				{
					// Get the value for the key, and tidy it up a little.
					var val = reader[key];
					ConsoleX.WriteLine("Field= \"" + key + "\", Value = " + val);
				}
				
//				var key = "Text3";
//				var val = reader[key];
//				ConsoleX.WriteLine("Field= \"" + key + "\", Value = " + val);
//				ConsoleX.WriteLine("Parsed as: " + reader.GetDateTimeValue(key).ToLongDateString());
//				
//				key = "Text4";
//				val = reader[key];
//				ConsoleX.WriteLine("Field= \"" + key + "\", Value = " + val);
//				ConsoleX.WriteLine("Parsed as: " + reader.GetDateTimeValue(key).ToLongDateString());
//				
//				key = "Text15";
//				val = reader[key];
//				ConsoleX.WriteLine("Field= \"" + key + "\", Value = " + val);
//				ConsoleX.WriteLine("Parsed as: " + reader.GetDateTimeValue(key).ToLongDateString());
				
				ConsoleX.WriteLine("Finished", ConsoleColor.Green);
				
				ConsoleX.WriteLine("Key to continue...");
				Console.ReadKey();
				
				reader = null;
			}
		}
		
		public void RunImportFiles()
		{
			var fileNames = this.GetFiles();
			if (fileNames != null)
			{
				foreach (string fileName in fileNames)
				{
					
					ConsoleX.WriteLine(string.Format("Reading '{0}' file...", fileName), ConsoleColor.Green);
					
					this.CurrentReader = new S82Reader(fileName);
					this.CurrentVolunteer = new Volunteer();
					
					bool skipProcessing = false;
					
					#region Step #1 Get name and gender
					
					ConsoleX.WriteLine("Step #1 Get name and gender", ConsoleColor.Green);
					
					this.Step1_NameAndGender();
					
					#endregion
					
					#region Step #2 Search for existing records (use existing or create new)
					
					ConsoleX.WriteLine("Step #2 Search for existing records", ConsoleColor.Green);
					
					this.Step2_SelectRecord(ref skipProcessing);
					
					#endregion
					
					#region Step #3 Read the rest of the form
					
					if(!skipProcessing)
					{
						ConsoleX.WriteLine("Step #3 Read the rest of the file", ConsoleColor.Green);
						
						this.Step3_ApplicationKind();
						this.Step3_FormsOfService();
						this.Step3_Dates();
						
						// TODO Read the rest of the file
						ConsoleX.WriteWarning("TODO Read the rest of the file");
					}
					
					#endregion
					
					#region Finish
					
					if(skipProcessing)
					{
						ConsoleX.WriteLine(string.Format("Skipping '{0}'", fileName), ConsoleColor.Red);
					}
					else
					{
						if(ConsoleX.WriteBooleanQuery("Shall I save to the database?"))
						{
							this.CurrentVolunteer.SaveToDatabase();
							
							if(this.CurrentVolunteer.ID == 0)
								ConsoleX.WriteLine("DONE: Inserted a new record to the database, using the data from the file!", ConsoleColor.Magenta);
							else
								ConsoleX.WriteWarning("TODO Update the record to the database, using the data from the file");
						}
						ConsoleX.WriteLine(string.Format("Finished '{0}'", fileName), ConsoleColor.Green);
					}
					
					#endregion
					
					this.CurrentReader = null;
					this.CurrentVolunteer = null;
					this.CloseFileIfOpen();
					
				}
			}
			
			ConsoleX.WriteLine("All files completed!");
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
		
		private void Step2_SelectRecord(ref bool skipProcessing)
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
					skipProcessing = true;
			}
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
			
			ConsoleX.WriteLine(string.Format("Application Kind = {0}", this.CurrentVolunteer.ApplicationKind.GetName()));
		}
		
		private void Step3_FormsOfService()
		{
			var constructionInput = this.CurrentReader.GetCheckBoxValue("Check Box3");
			var disasterInput = this.CurrentReader.GetCheckBoxValue("Check Box4");
			
			if(constructionInput)
				this.CurrentVolunteer.FormsOfService = this.CurrentVolunteer.FormsOfService | FormOfServiceKinds.HallConstruction;
			
			if(disasterInput)
				this.CurrentVolunteer.FormsOfService = this.CurrentVolunteer.FormsOfService | FormOfServiceKinds.DisasterRelief;
			
			ConsoleX.WriteLine("Forms of service: ", false);
			if(this.CurrentVolunteer.FormsOfService.HasFlag(FormOfServiceKinds.HallConstruction))
				ConsoleX.WriteLine("* Hall Construction", false);
			if(this.CurrentVolunteer.FormsOfService.HasFlag(FormOfServiceKinds.DisasterRelief))
				ConsoleX.WriteLine("* Disaster Relief", false);
			ConsoleX.WriteLine("", false);
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
			
			ConsoleX.WriteLine("Date of birth: " + this.CurrentVolunteer.DateOfBirth.ToLongDateString());
			ConsoleX.WriteLine("Date of baptism: " + this.CurrentVolunteer.DateOfBaptism.ToLongDateString());
			
		}
		
		#endregion
		
		#region Reusable Methods
		
		public string[] GetFiles()
		{
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
			this.OpenFileForHelp();
		}
		
		public void OpenFileForHelp()
		{
			if(this.OpenFileProcess == null)
			{
				ConsoleX.WriteWarning("I'll open the file for you now.");
				this.OpenFileProcess = Process.Start(this.CurrentReader.FilePath);
			}
		}
		
		public void CloseFileIfOpen()
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
