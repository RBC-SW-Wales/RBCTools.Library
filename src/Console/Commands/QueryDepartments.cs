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
		}
		
		public override void Run()
		{
			AccessFileHelper.CheckForAccessFile(base.ConsoleX);
			
			ConsoleX.WriteIntro(base.Description);
			
			var table = Departments.GetDepartmentsTable();
			ConsoleX.WriteDataTable(table, 30);
			
			ConsoleX.WriteHorizontalRule();
		}
	}
}
