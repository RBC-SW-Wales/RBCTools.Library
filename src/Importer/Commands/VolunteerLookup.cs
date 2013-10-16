using System;
using System.Data;
using RbcVolunteerApplications.Library.Database;

namespace RbcVolunteerApplications.Importer.Commands
{
	public class VolunteerLookup : CommandBase
	{
		public VolunteerLookup()
		{
			base.Slug = "volunteer-lookup";
			base.Description = "Lookup a Volunteer based on First and Last Name";
		}
		
		public override void Run()
		{
			var firstName = ConsoleX.WriteQuery("Please enter First Name:");
			var lastName = ConsoleX.WriteQuery("Please enter Last Name:");
			
			ConsoleX.WriteLine("Searching...");
			
			var table = Volunteers.GetByNames(firstName, lastName);
			
			if(table.Rows.Count == 0)
			{
				ConsoleX.WriteLine("No matched found on both First Name and Last Name! Trying just Last Name...");
				table = Volunteers.GetByLastName(lastName);
			}
			
			if(table.Rows.Count > 0)
			{
				ConsoleX.WriteLine(table.Rows.Count > 1 ? "The following matches where found:" : "The following match was found");
				ConsoleX.WriteDataTable(table);
			}
			else
			{
				ConsoleX.WriteLine("No matches found!");
			}
			
			ConsoleX.WriteHorizontalRule();
		}
	}
}
