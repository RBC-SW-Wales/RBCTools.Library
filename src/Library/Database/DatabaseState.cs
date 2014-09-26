using System;
using System.Runtime.InteropServices;
using Access = Microsoft.Office.Interop.Access;

namespace RbcTools.Library.Database
{
	public static class DatabaseState
	{
		
		public static bool IsDataSynchronised()
		{
			// First test the ActivityLog for an unsynchronised record.
			var synchronised = IsActivityLogSynchronised();
			
			// If that's okay, then test the Volunteers table for an unsynchronised record.
			if(synchronised)
				synchronised = IsVolunteersSynchronised();
			
			return synchronised;
		}
		
		private static bool IsActivityLogSynchronised()
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
		
		private static bool IsVolunteersSynchronised()
		{
			var connector = new Connector(" SELECT " +
			                              " Volunteers.ID, " +
			                              " Volunteers.FirstName, " +
			                              " Volunteers.MiddleName, " +
			                              " Volunteers.Surname " +
			                              " FROM Volunteers " +
			                              " WHERE Volunteers.ID = -1 " +
			                              " ORDER BY Volunteers.Surname ");
			
			var synced = connector.ExecuteDataTable().Rows.Count == 0;
			
			connector = null;
			
			return synced;
		}
		
		public static bool TrySync()
		{
			var trySync = false;
			var app = new Access.ApplicationClass();
			app.OpenCurrentDatabase(AccessFileDownloader.AccessFilePath);
			try
			{
				app.DoCmd.RunCommand(Access.AcCommand.acCmdSyncWebApplication);
				trySync = true;
			}
			catch (COMException){}
			finally
			{
				app.CloseCurrentDatabase();
				app.Quit(Access.AcQuitOption.acQuitSaveAll);
			}
			app = null;
			return trySync;
		}
	}
}
