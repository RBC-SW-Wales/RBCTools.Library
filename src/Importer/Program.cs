
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using RbcVolunteerApplications.Importer.Commands;
using RbcVolunteerApplications.Library;
using RbcVolunteerApplications.Library.Database;

namespace RbcVolunteerApplications.Importer
{
	class Program
	{
		public static List<CommandBase> CommandList = BuildCommandList();
		
		public static ConsoleX ConsoleX = new ConsoleX();
		
		private static List<CommandBase> BuildCommandList()
		{
			var list = new List<CommandBase>();
			list.Add(new ImportFiles());
//			list.Add(new CongregationLookup());
			list.Add(new VolunteerLookup());
			list.Add(new QueryVolunteers());
//			list.Add(new QueryDepartments());
			list.Add(new HelpCommand());
			return list;
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			// Get size right
			Console.SetWindowSize(Console.LargestWindowWidth - 2, Console.LargestWindowHeight);
			Console.BufferWidth = Console.WindowWidth;
			
			// Display application title
			ConsoleX.WriteTitle("RBC Application Form (S82) Importer");
			ConsoleX.WriteLine("Enter a command to start (e.g. 'help')");
			
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
									command.Run();
								}
							}
							if(!commandFound)
								ConsoleX.WriteLine("Command not found. Please try again.");
						}
					}
					catch (Exception ex)
					{
						ConsoleX.WriteException(ex);
					}
				}
			}
			
			ConsoleX.WriteLine("Good bye!");
			Thread.Sleep(1000);
		}
		
	}
}