
using System;

namespace RbcTools.Library.Database
{
	public static class ActivityLog
	{
		public static void SaveEntry(string description)
		{
			var query = ("INSERT INTO ActivityLog (Description) " + 
			             "VALUES (@Description)");
			var conn = new Connector(query);
			conn.AddParameter("@Description", description);
			conn.ExecuteNonQuery();
			conn = null;
		}
		
		public static void DeleteUnsyncronisedEntry()
		{
			var conn = new Connector("DELETE FROM ActivityLog WHERE ActivityLog.ID = -1");
			conn.ExecuteNonQuery();
			conn = null;
		}
		
		public static bool IsDataSynchronsed()
		{
			var connector = new Connector(" SELECT " +
			                              " ActivityLog.ID, " +
			                              " ActivityLog.LogDateTime, " +
			                              " ActivityLog.Description " +
			                              " FROM ActivityLog " +
			                              " WHERE ActivityLog.ID = -1 " +
			                              " ORDER BY ActivityLog.LogDateTime ");
			
			var synced = connector.ExecuteDataTable().Rows.Count == 0;
			
			connector = null;
			
			return synced;
		}
	}
}
