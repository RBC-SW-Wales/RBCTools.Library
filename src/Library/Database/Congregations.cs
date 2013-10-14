using System;

namespace RbcVolunteerApplications.Library.Database
{
	public static class Congregations
	{
		public static string GetNameById(int congregationId)
		{
			string name = "";
			
			var connector = new Connector("SELECT CongregationName FROM Congregation WHERE ID = @CongregationId");
			connector.AddParameter("@CongregationId", congregationId);
			var table = connector.ExecuteDataTable();
			if(table.Rows.Count == 1)
				name = (string)table.Rows[0]["CongregationName"];
			
			return name;
		}
	}
}
