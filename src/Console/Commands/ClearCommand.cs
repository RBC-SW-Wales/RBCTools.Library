using System;
using RbcConsole.Helpers;

namespace RbcConsole.Commands
{
	public class ClearCommand : CommandBase
	{
		public ClearCommand()
		{
			base.Slug = "clear";
			base.Description = "Clears the screen back to the starting text.";
			base.SkipIntroAndRule = true;
		}
		
		public override void Run()
		{
			Console.Clear();
			StartUpHelper.ShowStartUpMessage(ConsoleX);
		}
	}
}
