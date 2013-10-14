using System;
using System.Data;

namespace RbcVolunteerApplications.Library.Database
{
	public static class Volunteers
	{
		
		public static void DisplayAllVolunteers(Action<string> outputLine, Action<string> outputWarning)
		{
			var table = GetAllVolunteers(outputLine, outputWarning);
				
			if(table != null)
			{
				
				outputLine(new string('-', 93));
				string line = "";
				foreach(DataColumn column in table.Columns)
				{
					line += string.Format("| {0,-20} ", column.ColumnName);
				}
				line += "|";
				outputLine(line);
				
				outputLine(new string('-', 93));
				
				
				foreach(DataRow row in table.Rows)
				{
					line = "";
					foreach(DataColumn col in table.Columns)
					{
						line += string.Format("| {0,-20} ", row[col.ColumnName]);
					}
					line += "|";
					outputLine(line);
				}
				
				outputLine(new string('-', 93));
			}
		}
		
		public static DataTable GetAllVolunteers(Action<string> outputLine, Action<string> outputWarning)
		{
			DataTable table = null;
			try
			{
				var connector = new Connector("SELECT ID, FirstName, MiddleName, Surname, CongregationName FROM Volunteers WHERE Surname LIKE 'A%'");
				table = connector.ExecuteDataTable();
				connector = null;
			}
			catch (Exception ex)
			{
				outputWarning("Error! " + ex.Message);
				outputLine("Exception type: " + ex.GetType().ToString());
				outputLine("Stack trace: " + ex.StackTrace);
			}
			return table;
		}
		
	}
}
