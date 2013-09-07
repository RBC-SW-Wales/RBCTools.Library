
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
			Console.WriteLine("First you will need to choose the S82 PDF files that you wish to import.");
			Console.WriteLine("Press any key to continue to select files...");
			Console.ReadKey(true);
			
			var failures = new Dictionary<string, List<string>>();
			
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Multiselect = true;
			dlg.Title = "Choose S82 PDF files";
			dlg.Filter = "PDF Files|*.pdf";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				foreach(string str in dlg.FileNames)
				{
					FileInfo file = new FileInfo(str);
					Console.WriteLine(string.Format("Importing file: {0} (not really, soon though!)", file.Name));
					var reader = new S82Reader(str);
					if(!reader.ReadSuccessful)
					{
						failures.Add(file.Name, reader.ProblemList);
					}
					else
					{
						var application = reader.VolunteerApplication;
						// TODO Save the application to the database.
					}
					
				}
			}
			
			// TODO List files that failed to import.
			Console.WriteLine("The following files could not be imported for the reasons stated.");
			Console.WriteLine("Please correct problems and try again");
			
			foreach(var item in failures)
			{
				Console.WriteLine("File: " + item.Key);
				Console.WriteLine("Problems:");
				foreach(var problem in item.Value) Console.WriteLine(" * " + problem);
			}
			
			Console.WriteLine("Process complete.");
			
			Console.Write("Press any key to close...");
			Console.ReadKey(true);
		}
	}
}