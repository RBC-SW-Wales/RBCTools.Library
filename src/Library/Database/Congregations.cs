using System;
using System.Data;

namespace RbcTools.Library.Database
{
	public static class Congregations
	{
		public static DataTable GetById(int congregationId)
		{
			var connector = new Connector("SELECT ID, CongregationNo, CongregationName FROM Congregation WHERE ID = @CongregationId");
			connector.AddParameter("@CongregationId", congregationId);
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
		public static DataTable GetAll()
		{
			var connector = new Connector("SELECT ID, CongregationNo, CongregationName FROM Congregation ORDER BY CongregationName");
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
	}
}
