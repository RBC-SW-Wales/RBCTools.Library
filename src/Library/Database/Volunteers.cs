using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using RbcTools.Library.Badges;
using Access = Microsoft.Office.Interop.Access;

namespace RbcTools.Library.Database
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
		
		public static DataTable GetByBothNames(string firstName, string lastName, GenderKind gender)
		{
			var connector = new Connector(BasicQuery +
			                              " WHERE Volunteers.FirstName = @FirstName " +
			                              " AND Volunteers.Surname = @Surname " +
			                              " AND Volunteers.Gender = @Gender " +
			                              " ORDER BY Volunteers.Surname ");
			connector.AddParameter("@FirstName", firstName);
			connector.AddParameter("@Surname", lastName);
			connector.AddParameter("@Gender", gender.ToString());
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
		public static DataTable GetByEitherName(string firstName, string lastName, GenderKind gender)
		{
			var connector = new Connector(BasicQuery +
			                              " WHERE " +
			                              " (Volunteers.FirstName = @FirstName OR Volunteers.Surname = @Surname)" +
			                              " AND Volunteers.Gender = @Gender " +
			                              " ORDER BY Volunteers.Surname ");
			connector.AddParameter("@FirstName", firstName);
			connector.AddParameter("@Surname", lastName);
			connector.AddParameter("@Gender", gender.ToString());
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
		public static DataTable GetByLastName(string lastName, GenderKind gender)
		{
			var connector = new Connector(BasicQuery +
			                              " WHERE Volunteers.Surname = @Surname AND Volunteers.Gender = @Gender " +
			                              " ORDER BY Volunteers.Surname ");
			connector.AddParameter("@Surname", lastName);
			connector.AddParameter("@Gender", gender.ToString());
			var table = connector.ExecuteDataTable();
			connector = null;
			return table;
		}
		
		public static bool IsDataSynchronsed()
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
		
		public static List<Badge> GetBadgesByDepartment(Department department)
		{
			var connector = new Connector(" SELECT Volunteers.*, Trades.Trade AS TradeName, Congregation.CongregationName AS Congregation " +
			                              " FROM (Volunteers INNER JOIN Trades ON (Trades.ID = Volunteers.Trade)) INNER JOIN Congregation ON (Congregation.ID = Volunteers.CongregationName) " +
			                              " WHERE Trades.ID = @DepartmentId " +
			                              " ORDER BY Volunteers.Surname");
			connector.AddParameter("@DepartmentID", department.ID);
			var table = connector.ExecuteDataTable();
			
			var badges = new List<Badge>();
			foreach(DataRow row in table.Rows)
			{
				badges.Add(new Badge(row));
			}
			
			connector = null;
			return badges;
		}
		
		public static Volunteer GetByID(int volunteerId)
		{
			Volunteer volunteer = null;
			
			var connector = new Connector(BasicQuery +
			                              " WHERE Volunteers.ID = @VolunteerID " +
			                              " ORDER BY Volunteers.Surname ");
			
			connector.AddParameter("@VolunteerID", volunteerId);
			
			var table = connector.ExecuteDataTable();
			connector = null;
			
			if(table.Rows.Count == 1)
			{
				volunteer = new Volunteer(table.Rows[0]);
			}
			
			return volunteer;
		}
		
		private static string BasicQuery
		{
			get
			{
				return "" +
					" SELECT " +
					" Volunteers.ID, " +
					" Volunteers.FirstName, " +
					" Volunteers.MiddleName, " +
					" Volunteers.Surname, " +
					" Volunteers.Gender, " +
					" Congregation.CongregationName " +
					" FROM Volunteers " +
					" LEFT JOIN Congregation ON (Congregation.ID = Volunteers.CongregationName) ";
			}
		}
		
	}
}
