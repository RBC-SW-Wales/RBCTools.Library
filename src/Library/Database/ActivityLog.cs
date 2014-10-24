
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
	}
}
