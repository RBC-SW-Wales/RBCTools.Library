using System;
using System.Data;

namespace RbcVolunteerApplications.Library.Database
{
	public static class Volunteers
	{
		
		public static DataTable GetAllVolunteers()
		{
			DataTable table = null;
			var connector = new Connector("SELECT ID, FirstName, MiddleName, Surname, CongregationName " +
			                              "FROM Volunteers " +
			                              "WHERE Surname LIKE 'A%'" +
			                              "ORDER BY Surname");
			table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
	}
}
