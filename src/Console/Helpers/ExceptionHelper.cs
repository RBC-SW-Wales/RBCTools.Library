using System;
using System.Diagnostics;

namespace RbcConsole.Helpers
{
	public static class ExceptionHelper
	{
		public static void HandleException(Exception ex, ConsoleX consoleX)
		{
			consoleX.WriteException(ex);
			if(consoleX.WriteBooleanQuery("Would you like to report this error?"))
			{
				consoleX.WriteLine("Ok. I'll compose an email for you.");
				consoleX.WriteLine("This should open in your default email client. Please send the email.");
				string mailto = "mailto:itsupport@rbcwales.org?subject=Error report: {0}&body={1}";
				string body = "Error: " + ex.Message + Environment.NewLine;
				body += "Type: " + ex.GetType().ToString() + Environment.NewLine;
				body += "Stack Trace: " + Environment.NewLine + ex.StackTrace;
				mailto = string.Format(mailto, ex.Message, body);
				mailto = Uri.EscapeUriString(mailto);
				Process.Start(mailto);
			}
			consoleX.WriteHorizontalRule();
		}
	}
}
