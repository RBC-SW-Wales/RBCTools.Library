
using System;

namespace RbcVolunteerApplications.Importer.Commands
{
	public abstract class CommandBase
	{
		public CommandBase(){}
		
		public ConsoleX ConsoleX = new ConsoleX();
		
		public string Slug { get; set; }
		
		public string Description { get; set; }
		
	 	public abstract void Run();
	}
}
