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
			ConsoleX.WriteIntro("Open and fields in files");
			
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
					
					ConsoleX.WriteLine("Step #2 Match with existing record", ConsoleColor.Green);
					
					ConsoleX.WriteLine(string.Format("Looking up '{0} {1}'...", newApplication.FirstName, newApplication.LastName));
					
					bool matchesFound = VolunteerLookup.TrySearchForNames(newApplication.FirstName, newApplication.LastName, ConsoleX);
					
					VolunteerApplication existingVolunteer = null;
					
					if(matchesFound)
					{
						var input = ConsoleX.WriteQuery("Do you want to update an existing record? If so, please enter the ID, or press ENTER");
						
						int inputId;
						if(int.TryParse(input, out inputId))
						{
							// TODO Get record from db.
							ConsoleX.WriteWarning("TODO Get record from db.");
							existingVolunteer = Volunteers.GetByID(inputId);
							
							if(existingVolunteer != null)
							{
							ConsoleX.WriteWarning(string.Format("ID: {0}, FirstName: {1}, LastName {2}",
							                                    existingVolunteer.ID,
							                                    existingVolunteer.FirstName,
							                                    existingVolunteer.LastName));
							}
							else
								ConsoleX.WriteWarning("No one found with that ID");
						}
					}
					
					if(existingVolunteer == null)
					{
						ConsoleX.WriteWarning("TODO Do insert and return record from db for further updates.");
						// TODO Do insert and return record from db for further updates.
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
