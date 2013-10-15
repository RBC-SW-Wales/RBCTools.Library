
using System;
using System.Data;

namespace RbcVolunteerApplications.Library.Database
{
	public static class Departments
	{
		public static DataTable GetDepartmentsTable()
		{
			var connector = new Connector("SELECT ID, Trade " +
			                              "FROM Trades " +
			                              "ORDER BY Trade");
			DataTable table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
	}
}
