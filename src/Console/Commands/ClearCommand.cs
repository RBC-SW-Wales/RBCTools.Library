using System;

namespace RbcConsole.Commands
{
	public class ClearCommand : CommandBase
	{
		public ClearCommand()
		{
			base.Slug = "clear";
			base.Description = "Clears the screen back to the starting text.";
		}
		
		public override void Run()
		{
			Console.Clear();
			// Display application title
			ConsoleX.WriteTitle("RBC Application Form (S82) Importer");
			ConsoleX.WriteLine("Enter a command to start (e.g. 'help')");
		}
	}
}
