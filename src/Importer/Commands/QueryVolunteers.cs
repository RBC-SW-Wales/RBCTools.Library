
using System;
using RbcVolunteerApplications.Library.Database;

namespace RbcVolunteerApplications.Importer.Commands
{
	/// <summary>
	/// Description of QueryVolunteers.
	/// </summary>
	public class QueryVolunteers : CommandBase
	{
		public QueryVolunteers()
		{
			base.Slug = "query-volunteers";
			base.Description = "Query the Volunteers database";
		}
		
		public override void Run()
		{
			ConsoleX.WriteIntro("Test Database Connection, SELECT data");
			Volunteers.DisplayAllVolunteers(ConsoleX.WriteLine, ConsoleX.WriteWarning);
			ConsoleX.WriteHorizontalRule();
		}
	}
}
