using System;
using System.Data;
using RbcConsole.Helpers;
using RbcTools.Library;
using RbcTools.Library.Database;

namespace RbcConsole.Commands
{
	public class VolunteerLookup : CommandBase
	{
		public VolunteerLookup()
		{
			base.Slug = "volunteer-lookup";
			base.Description = "Lookup a Volunteer based on First and Last Name";
			base.IsDatabaseCommand = true;
		}
		
		public override void Run()
		{
			var firstName = ConsoleX.WriteQuery("Please enter First Name:");
			var lastName = ConsoleX.WriteQuery("Please enter Last Name:");
			var isMale = ConsoleX.WriteBooleanQuery("Gender: Are they male?");
			
			var vol = new Volunteer();
			vol.FirstName = firstName;
			vol.LastName = lastName;
			vol.Gender = isMale ? GenderKind.Male : GenderKind.Female;
			
			TrySearchForNames(vol, ConsoleX);
		}
		
		public static bool TrySearchForNames(Volunteer volunteer, ConsoleX consoleX)
		{
			// TODO Use Gender also to help with match.
			bool matchesFound = true;
			
			consoleX.WriteLine("Searching for matches on both First and Last name...");
			
			var table = Volunteers.GetByBothNames(volunteer.FirstName, volunteer.LastName, volunteer.Gender);
			
			if(table.Rows.Count == 0)
			{
				consoleX.WriteLine("No matched found on both First and Last name! Trying a wider search:");
				if(volunteer.Gender == GenderKind.Male)
				{
					consoleX.WriteLine("Searching for other brothers with same Last name...");
					table = Volunteers.GetByLastName(volunteer.LastName, volunteer.Gender);
				}
				else
				{
					consoleX.WriteLine("Searching for other sisters with same First OR Last name...");
					table = Volunteers.GetByEitherName(volunteer.FirstName, volunteer.LastName, volunteer.Gender);
				}
			}
			
			if(table.Rows.Count > 0)
			{
				consoleX.WriteLine(table.Rows.Count > 1 ? "The following matches where found:" : "The following match was found");
				consoleX.WriteDataTable(table, 15);
			}
			else
			{
				matchesFound = false;
				consoleX.WriteLine("No matches found!");
			}
			return matchesFound;
		}
	}
}
