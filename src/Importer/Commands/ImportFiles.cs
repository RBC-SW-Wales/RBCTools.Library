using System;
using System.Windows.Forms;
using RbcVolunteerApplications.Library;

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
					var reader = new S82Reader(str);
					
					var application = reader.BuildVolunteerApplication(ConsoleX.WriteWarning, ConsoleX.WriteQuery);
					
					ConsoleX.WriteLine("Read the file. Here's the summary.");
					ConsoleX.WriteLine("Last Name: " + application.LastName);
					ConsoleX.WriteLine("First Name: " + application.FirstName);
					ConsoleX.WriteLine("Middle Names: " + application.MiddleNames);
					ConsoleX.WriteLine("Read the file, and ready to save into database! Press any key to continue.");
					Console.ReadKey(true);
					
					ConsoleX.WriteLine("Inserting into database");
					application.InsertIntoDatabase();
					ConsoleX.WriteLine("Complete.");
					
					//reader.ShowFields(ConsoleX.WriteLine);
				}
			}
			
			ConsoleX.WriteLine("All files completed!");
			ConsoleX.WriteHorizontalRule();
		}
	}
}
