using System;
using System.Data;
using RbcConsole.Helpers;
using RbcTools.Library;
using RbcTools.Library.Database;

namespace RbcConsole.Commands
{
	public class QueryDepartments : CommandBase
	{
		public QueryDepartments()
		{
			base.Slug = "query-departments";
			base.Description = "Query the Departments in the database";
			base.IsDatabaseCommand = true;
		}
		
		public override void Run()
		{
			var table = Departments.GetDepartmentsTable();
			ConsoleX.WriteDataTable(table, 30);
		}
	}
}
