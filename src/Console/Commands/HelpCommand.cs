using System;

namespace RbcConsole.Commands
{
	public class HelpCommand : CommandBase
	{
		public HelpCommand()
		{
			base.Slug = "help";
			base.Description = "Help on using this tool";
		}
		
		public override void Run()
		{
			ConsoleX.WriteLine("Available commands are:");
			foreach(var command in Program.CommandList)
			{
				ConsoleX.WriteLine(string.Format("{0, -20} - {1}", command.Slug, command.Description));
			}
			ConsoleX.WriteLine(string.Format("{0, -20} - {1}", "exit", "Exit/close the application"));
		}
	}
}
