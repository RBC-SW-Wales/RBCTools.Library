using System;
using System.Data;

namespace RbcVolunteerApplications.Library.Database
{
	public static class Volunteers
	{
		
		public static DataTable GetAllVolunteers()
		{
			DataTable table = null;
			var connector = new Connector(BasicQuery +
			                              " WHERE Volunteers.Surname LIKE 'A%' " +
			                              " ORDER BY Volunteers.Surname ");
			table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
		public static DataTable GetByNames(string firstName, string lastName)
		{
			var connector = new Connector(BasicQuery +
			                              " WHERE Volunteers.FirstName = @FirstName AND Volunteers.Surname = @Surname " +
			                              " ORDER BY Volunteers.Surname ");
			connector.AddParameter("@FirstName", firstName);
			connector.AddParameter("@Surname", lastName);
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
		public static DataTable GetByLastName(string lastName)
		{
			var connector = new Connector(BasicQuery +
			                              " WHERE Volunteers.Surname = @Surname " +
			                              " ORDER BY Volunteers.Surname ");
			connector.AddParameter("@Surname", lastName);
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
		private static string BasicQuery
		{
			get
			{
				return "" +
					" SELECT Volunteers.ID, Volunteers.FirstName, Volunteers.MiddleName, Volunteers.Surname, Congregation.CongregationName " +
					" FROM Volunteers " +
					" INNER JOIN Congregation ON (Congregation.ID = Volunteers.CongregationName) ";
			}
		}
		
	}
}
