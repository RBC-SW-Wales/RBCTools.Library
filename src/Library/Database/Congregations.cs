using System;
using System.Data;

namespace RbcTools.Library.Database
{
	public static class Congregations
	{
		public static DataTable GetById(int congregationId)
		{
			var connector = new Connector("SELECT ID, CongregationName, CongregationNo FROM Congregation WHERE ID = @CongregationId");
			connector.AddParameter("@CongregationId", congregationId);
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
	}
}
