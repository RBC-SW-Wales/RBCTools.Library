using System;
using RbcTools.Library;

namespace RbcConsole.Commands
{
	public class ShowFileFields : CommandBase
	{
		public ShowFileFields()
		{
			base.Slug = "show-file-fields";
			base.Description = "Show fields from S82 files";
		}
		
		public override void Run()
		{
			ConsoleX.WriteIntro(base.Description);
			
			this.ShowFields();
			
			ConsoleX.WriteHorizontalRule();
		}
		
		public void ShowFields()
		{
			var fileNames = ImportFiles.GetFiles(base.ConsoleX);
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
		
	}
}
