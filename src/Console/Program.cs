
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using RbcConsole.Commands;
using RbcConsole.Helpers;
using RbcTools.Library;
using RbcTools.Library.Database;

namespace RbcConsole
{
	class Program
	{
		public static List<CommandBase> CommandList = BuildCommandList();
		
		public static ConsoleX ConsoleX = new ConsoleX();
		
		private static List<CommandBase> BuildCommandList()
		{
			var list = new List<CommandBase>();
			list.Add(new ImportFiles());
//			list.Add(new ShowFileFields());
//			list.Add(new CongregationLookup());
			list.Add(new ListCongregations());
//			list.Add(new VolunteerLookup());
//			list.Add(new QueryVolunteers());
//			list.Add(new QueryDepartments());
			list.Add(new HelpCommand());
			list.Add(new ClearCommand());
			return list;
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			// Get size right
			Console.SetWindowSize(Console.LargestWindowWidth - 2, Console.LargestWindowHeight);
			Console.BufferWidth = Console.WindowWidth;
			
			// Display start message
			StartUpHelper.ShowStartUpMessage(ConsoleX);
			
			// Enter into loop that takes commands.
			var input = "";
			while(input != "exit")
			{
				input = ConsoleX.ReadPromt(Program.CommandList.ListSlugs());
				if(input != "exit")
				{
					try
					{
						if(!string.IsNullOrEmpty(input))
						{
							var commandFound = false;
							foreach(var command in Program.CommandList)
							{
								if(command.Slug == input)
								{
									commandFound = true;
									command.Execute();
								}
							}
							if(!commandFound)
								ConsoleX.WriteLine("Command not found. Please try again.");
						}
					}
					catch (Exception ex)
					{
						ConsoleX.WriteException(ex);
						if(ConsoleX.WriteBooleanQuery("Would you like to report this error?"))
						{
							ConsoleX.WriteLine("Ok. I'll compose an email for you.");
							ConsoleX.WriteLine("This should open in your default email client. Please send the email.");
							string mailto = "mailto:itsupport@rbcwales.org?subject=Error report: {0}&body={1}";
							string body = "Error: " + ex.Message + Environment.NewLine;
							body += "Type: " + ex.GetType().ToString() + Environment.NewLine;
							body += "Stack Trace: " + Environment.NewLine + ex.StackTrace;
							mailto = string.Format(mailto, ex.Message, body);
							mailto = Uri.EscapeUriString(mailto);
							Process.Start(mailto);
						}
						ConsoleX.WriteHorizontalRule();
					}
				}
			}
			
			ConsoleX.WriteLine("Good bye!");
			Thread.Sleep(1000);
		}
		
	}
}