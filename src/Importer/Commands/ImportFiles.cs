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
		
		public override void Run()
		{
			ConsoleX.WriteIntro(base.Description);
			
			ConsoleX.WriteLine("First you will need to choose the S82 PDF files that you wish to import.");
			ConsoleX.WriteLine("Press any key to continue to select files...");
			Console.ReadKey(true);
			
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Multiselect = true;
			dlg.Title = "Choose S82 PDF files";
			dlg.Filter = "PDF Files|*.pdf";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				foreach (string str in dlg.FileNames)
				{
					ConsoleX.WriteLine(string.Format("Reading '{0}' file...", str));
					
					var reader = new S82Reader(str);
					
					#region Step #1 Get Name
					
					ConsoleX.WriteLine("Step #1 Get name", ConsoleColor.Green);
					
					var newApplication = this.GetVolunteerName(reader);
					
					ConsoleX.WriteLine(string.Format("Volunteer's name is {0} {1}", newApplication.FirstName, newApplication.LastName));
					
					#endregion
					
					#region Step #2 Search for existing records (use existing or create new)
					
					ConsoleX.WriteLine("Step #2 Search for existing records", ConsoleColor.Green);
					
					ConsoleX.WriteLine(string.Format("Looking up '{0} {1}'...", newApplication.FirstName, newApplication.LastName));
					
					bool matchesFound = VolunteerLookup.TrySearchForNames(newApplication.FirstName, newApplication.LastName, ConsoleX);
					
					VolunteerApplication existingVolunteer = null;
					
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
								var vol = Volunteers.GetByID(possibleID);
								if(vol != null)
								{
									ConsoleX.WriteLine(string.Format("You have selected {0} - {1} {2}.",
									                                 vol.ID,
									                                 vol.FirstName,
									                                 vol.LastName));
									
									var confirm = ConsoleX.WriteQuery("Is this correct? Enter 'yes' to confirm, ENTER to try again.").ToLower();
									
									if(confirm == "yes")
									{
										input = string.Empty; // So that we can leave loop.
										existingVolunteer = vol;
										ConsoleX.WriteWarning("TODO Update the existing record, using the data from file.");
										// TODO Update the existing record, using the data from file.
									}
								}
								else
								{
									ConsoleX.WriteWarning("ID Incorrect, please try again.");
								}
							}
						} while(!string.IsNullOrEmpty(input));
						
						
					}
					
					if(existingVolunteer == null)
					{
//						ConsoleX.WriteLine("Creating a new record in the database...");
//						newApplication.SaveToDatabase();
						ConsoleX.WriteWarning("TODO Create a new record, using the data from file.");
						// TODO Create a new record, using the data from file.
					}
					
					#endregion
					
					//ConsoleX.WriteLine("Read the file, and ready to save into database! Press any key to continue.");
					//Console.ReadKey(true);
					
					//ConsoleX.WriteLine("Inserting into database");
					//application.InsertIntoDatabase();
					
					ConsoleX.WriteLine("Complete.");
					
					//reader.ShowFields(ConsoleX.WriteLine);
				}
			}
			
			ConsoleX.WriteLine("All files completed!");
			ConsoleX.WriteHorizontalRule();
		}
		
		private VolunteerApplication GetVolunteerName(S82Reader reader)
		{
			var volunteer = new VolunteerApplication();
			
			// Get Surname, FirstName and MiddleName
			
			string pdfValue = reader["Text2"];
			
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
				
				var process = Process.Start(reader.FilePath);
				
				lastName = ConsoleX.WriteQuery("Please can you tell me their 'Last Name'?");
				middleNames = ConsoleX.WriteQuery("Please can you tell me if they have any 'Middles Names'?");
				firstName = ConsoleX.WriteQuery("Please can you tell me their 'First Name'?");
				
				try
				{
					process.Kill();
				}
				catch (Exception){}
			}
			
			volunteer.LastName = lastName;
			volunteer.MiddleNames = middleNames;
			volunteer.FirstName = firstName;
			
			return volunteer;
		}
		
		public void ShowFields(S82Reader reader)
		{
			ConsoleX.WriteLine("Reading fields from " + reader.FilePath);
			foreach(var key in reader.Keys)
			{
				// Get the value for the key, and tidy it up a little.
				var val = reader[key];
				ConsoleX.WriteLine("Field= \"" + key + "\", Value = " + val);
			}
		}
		
	}
}
