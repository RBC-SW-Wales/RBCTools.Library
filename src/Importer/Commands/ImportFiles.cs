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
		
		private VolunteerApplication CurrentVolunteer;
		private S82Reader CurrentReader;
		
		public override void Run()
		{
			ConsoleX.WriteIntro(base.Description);
			
			this.RunImportFiles();
//			this.ShowFields();
			
			ConsoleX.WriteHorizontalRule();
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
					this.CurrentVolunteer = new VolunteerApplication();
					
					bool skipProcessing = false;
					
					#region Step #1 Get Name
					
					ConsoleX.WriteLine("Step #1 Get name", ConsoleColor.Green);
					
					this.Step1_Name();
					
					#endregion
					
					#region Step #2 Search for existing records (use existing or create new)
					
					this.Step2_SelectRecord(ref skipProcessing);
					
					#endregion
					
					#region Step #3 Read the rest of the form
					
					if(!skipProcessing)
					{
						this.Step3_ApplicationKind();
						
						// TODO Read the rest of the file
						ConsoleX.WriteWarning("TODO Read the rest of the file");
					}
					
					#endregion
					
					if(skipProcessing)
					{
						ConsoleX.WriteLine(string.Format("Skipping '{0}'", fileName), ConsoleColor.Red);
					}
					else
					{
						if(this.CurrentVolunteer.ID == 0)
							ConsoleX.WriteWarning("TODO Insert a new record to the database, using the data from the file");
						else
							ConsoleX.WriteWarning("TODO Update the record to the database, using the data from the file");
						
						this.CurrentVolunteer.SaveToDatabase();
						
						ConsoleX.WriteLine(string.Format("Finished '{0}'", fileName));
					}
					
					this.CurrentReader = null;
					this.CurrentVolunteer = null;
					
				}
			}
			
			ConsoleX.WriteLine("All files completed!");
		}
		
		private void Step1_Name()
		{
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
				ConsoleX.WriteWarning("I'm having problems understanding the 'names' field. I need your help. I'll open the file for you now.");
				
				var process = Process.Start(this.CurrentReader.FilePath);
				
				lastName = ConsoleX.WriteQuery("Please can you tell me their 'Last Name'?");
				middleNames = ConsoleX.WriteQuery("Please can you tell me if they have any 'Middles Names'?");
				firstName = ConsoleX.WriteQuery("Please can you tell me their 'First Name'?");
				
				try
				{
					process.Kill();
				}
				catch (Exception){}
			}
			
			this.CurrentVolunteer.LastName = lastName;
			this.CurrentVolunteer.MiddleNames = middleNames;
			this.CurrentVolunteer.FirstName = firstName;
			
			ConsoleX.WriteLine(string.Format("Volunteer's name is {0} {1}", this.CurrentVolunteer.FirstName, this.CurrentVolunteer.LastName));
		}
		
		private void Step2_SelectRecord(ref bool skipProcessing)
		{
			ConsoleX.WriteLine("Step #2 Search for existing records", ConsoleColor.Green);
			
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
			var newApplicationInput = this.CurrentReader["Check Box1"];
			var updateApplicationInput = this.CurrentReader["Check Box2"];
			
			bool isUpdate = false;
			
			if(string.IsNullOrEmpty(updateApplicationInput) && string.IsNullOrEmpty(newApplicationInput))
			{
				if(this.CurrentVolunteer.ID != 0)
					isUpdate = true;
			}
			else if(updateApplicationInput == "Yes")
			{
				isUpdate = true;
			}
			
			if(isUpdate)
				this.CurrentVolunteer.ApplicationKind = ApplicationKind.UpdateOfPersonalData;
			else
				this.CurrentVolunteer.ApplicationKind = ApplicationKind.NewApplication;
			
			ConsoleX.WriteLine(string.Format("Application Kind = {0}", this.CurrentVolunteer.ApplicationKind.GetName()));
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
				ConsoleX.WriteLine("Finished", ConsoleColor.Green);
				reader = null;
			}
		}
		
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
		
	}
}
