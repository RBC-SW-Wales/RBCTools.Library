using System;
using RbcTools.Library;

namespace RbcConsole.Helpers
{
	public static class StartUpHelper
	{
		public static void ShowStartUpMessage(IConsoleX consoleX)
		{
			// Display application title
			consoleX.WriteTitle("RBC Console, application for interfacing with RBC South and West Wales database");
			consoleX.WriteLine("Enter a command to start (e.g. 'help')");
		}
	}
}
