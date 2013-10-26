using System;
using RbcVolunteerApplications.Library.Database;

namespace RbcVolunteerApplications.Importer.Commands
{
	public class CongregationLookup : CommandBase
	{
		public CongregationLookup()
		{
			base.Slug = "cong-lookup";
			base.Description = "Lookup Congregation Details by ID";
		}
		
		public override void Run()
		{
			ConsoleX.WriteIntro("Congregation Look-up");
			ConsoleX.WriteLine("Enter Congregation ID, or 'exit'");
			
			var input = "";
			while(input != "exit")
			{
				
				input = ConsoleX.ReadPromt();
				
				if(input != "exit")
				{
					int congregationId;
					if(int.TryParse(input, out congregationId))
						ConsoleX.WriteDataTable(Congregations.GetById(congregationId));
					else
						ConsoleX.WriteWarning("Input didn't parse as an integer!");
				}
			}
			
			ConsoleX.WriteLine("Congregation lookup finished");
			ConsoleX.WriteHorizontalRule();
		}
	}
}
