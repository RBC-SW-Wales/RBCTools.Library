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
		
		public static DataTable GetByNames(string firstName, string lastName)
		{
			var connector = new Connector("SELECT ID, FirstName, MiddleName, Surname " +
			                              "FROM Volunteers " +
			                              "WHERE FirstName = @FirstName AND Surname = @Surname " +
			                              "ORDER BY Surname");
			connector.AddParameter("@FirstName", firstName);
			connector.AddParameter("@Surname", lastName);
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
		public static DataTable GetByLastName(string lastName)
		{
			var connector = new Connector("SELECT ID, FirstName, MiddleName, Surname " +
			                              "FROM Volunteers " +
			                              "WHERE Surname = @Surname " +
			                              "ORDER BY Surname");
			connector.AddParameter("@Surname", lastName);
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
	}
}
