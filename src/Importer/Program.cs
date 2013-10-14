
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using RbcVolunteerApplications.Library;
using RbcVolunteerApplications.Library.Database;

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
			
			Program.OpenFilesAndProcess();
			
//			Program.DisplaySelectedData();
			
//			Program.InsertTestVolunteer();
			
			ConsoleX.WriteLine("Press any key to close...");
			Console.ReadKey(true);
		}
		
		private static void OpenFilesAndProcess()
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
					
					var application = reader.BuildVolunteerApplication(ConsoleX.WriteQuery);
					
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
			
		}
		
		private static void DisplaySelectedData()
		{
			ConsoleX.WriteIntro("Test Database Connection, SELECT data");
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