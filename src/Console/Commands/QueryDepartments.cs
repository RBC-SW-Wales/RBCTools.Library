using System;
using System.Data;
using RbcVolunteerApplications.Library;
using RbcVolunteerApplications.Library.Database;

namespace RbcVolunteerApplications.Importer.Commands
{
	public class QueryDepartments : CommandBase
	{
		public QueryDepartments()
		{
			base.Slug = "query-departments";
			base.Description = "Query the Departments in the database";
		}
		
		public override void Run()
		{
			var table = Departments.GetDepartmentsTable();
			ConsoleX.WriteDataTable(table, 30);
		}
	}
}
