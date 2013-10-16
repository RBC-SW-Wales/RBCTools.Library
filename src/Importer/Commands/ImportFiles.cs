using System;
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
					
					ConsoleX.WriteLine("Step #1 Get name", ConsoleColor.Green);
					
					var newApplication = reader.GetVolunteerName(ConsoleX);
					
					ConsoleX.WriteLine(string.Format("Volunteer's name is {0} {1}", newApplication.FirstName, newApplication.LastName));
					
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
						ConsoleX.WriteLine("Creating a new record in the database...");
						newApplication.InsertIntoDatabase();
						ConsoleX.WriteWarning("TODO Create a new record, using the data from file.");
						// TODO Create a new record, using the data from file.
					}
					
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
	}
}
