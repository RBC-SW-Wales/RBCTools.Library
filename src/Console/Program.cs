
using System;
using System.Collections.Generic;
using System.Threading;

using RbcConsole.Commands;
using RbcConsole.Helpers;

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
						ExceptionHelper.HandleException(ex, ConsoleX);
					}
				}
			}
			
			ConsoleX.WriteLine("Good bye!");
			Thread.Sleep(1000);
		}
		
	}
}