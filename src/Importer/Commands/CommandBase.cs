
using System;

namespace RbcVolunteerApplications.Importer.Commands
{
	public abstract class CommandBase
	{
		public CommandBase(){}
		
		public string Slug { get; set; }
		
		public string Description { get; set; }
		
	 	public abstract void Run();
	}
}
