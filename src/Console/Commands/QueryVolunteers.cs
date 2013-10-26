
using System;
using RbcConsole.Helpers;
using RbcTools.Library.Database;

namespace RbcConsole.Commands
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
			base.IsDatabaseCommand = true;
		}
		
		public override void Run()
		{
			var table = Volunteers.GetAllVolunteers();
			ConsoleX.WriteDataTable(table);
		}
	}
}
