
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
			base.Description = "Query the Volunteers in the database";
		}
		
		public override void Run()
		{
			ConsoleX.WriteIntro("Query Volunteers");
			
			var table = Volunteers.GetAllVolunteers();
			ConsoleX.WriteDataTable(table);
			
			ConsoleX.WriteHorizontalRule();
		}
	}
}
