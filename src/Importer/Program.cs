
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using RbcVolunteerApplications.Library;

namespace RbcVolunteerApplications.Importer
{
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			// Get size right
			Console.SetWindowSize(Console.LargestWindowWidth - 2, Console.LargestWindowHeight);
			
			ConsoleX.WriteTitle("RBC Application Form (S82) Importer");
			
			Program.OpenAndProcessFiles();
			
//			Program.DisplaySelectedData();
			
//			Program.InsertTestVolunteer();
			
			ConsoleX.WriteLine("Press any key to close...");
			Console.ReadKey(true);
		}
		
		private static void OpenAndProcessFiles()
		{
			
			ConsoleX.WriteIntro("Open and process files");
			
			ConsoleX.WriteLine("First you will need to choose the S82 PDF files that you wish to import.");
			ConsoleX.WriteLine("Press any key to continue to select files...");
			Console.ReadKey(true);

			var failures = new Dictionary<string, List<string>>();

			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Multiselect = true;
			dlg.Title = "Choose S82 PDF files";
			dlg.Filter = "PDF Files|*.pdf";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				foreach (string str in dlg.FileNames)
				{
					FileInfo file = new FileInfo(str);
					ConsoleX.WriteLine(string.Format("Importing file: {0} (not really, soon though!)", file.Name));
					var reader = new S82Reader(str);
					if (!reader.ReadSuccessful)
					{
						failures.Add(file.Name, reader.ProblemList);
					}
					else
					{
						var application = reader.VolunteerApplication;
					}
					// TODO Save the application to the database.
				}
			}

			// TODO List files that failed to import.
			ConsoleX.WriteWarning("The following files could not be imported for the reasons stated.");
			ConsoleX.WriteLine("Please correct problems and try again");

			foreach (var item in failures)
			{
				ConsoleX.WriteLine("File: " + item.Key);
				ConsoleX.WriteLine("Problems:");
				foreach (var problem in item.Value)
					ConsoleX.WriteWarning(" * " + problem);
			}

			ConsoleX.WriteLine("Process complete.");
		}
		
		private static void DisplaySelectedData()
		{
			ConsoleX.WriteLine("Test Database Connection");
			Connector.SelectTest(ConsoleX.WriteLine, ConsoleX.WriteWarning);
			ConsoleX.WriteLine("Database select completed.");
		}
		
		private static void InsertTestVolunteer()
		{
			ConsoleX.WriteIntro("Test VolunteerApplication InsertIntoDatabase");
			
			try
			{
				var application = new VolunteerApplication();
				application.FirstName = "Bob";
				application.MiddleNames = "Test Volunteer";
				application.LastName = "Aaatest";
				application.InsertIntoDatabase();
				application = null;
				ConsoleX.WriteLine("Insert worked");
			}
			catch (Exception ex)
			{
				ConsoleX.WriteWarning("Error! " + ex.Message);
				throw;
			}
		}
		
	}
}