using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;

namespace RbcVolunteerApplications.Library
{
	public class Connector
	{
		#region Contructors
		
		public Connector(string query)
		{
			this._Query = query;
			this.Connection = new OleDbConnection(this.ConnectionString);
			this.Command = new OleDbCommand(query, this.Connection);
		}
		
		#endregion
		
		#region Fields
		
		private string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Phil\Dropbox\Theocratic\RBC\Database\RBCWalesVolsDatabase.accdb";
		private OleDbConnection Connection;
		private OleDbCommand Command;
		
		private string _Query;
		
		#endregion
		
		#region Properties
		
		public string Query
		{
			get { return _Query; }
			set
			{
				if(value != _Query)
				{
					this.Command.CommandText = value;
					_Query = value;
				}
			}
		}
		
		#endregion
		
		public static void SelectTest(Action<string> outputLine, Action<string> outputWarning)
		{
			try
			{
				var connector = new Connector("SELECT ID, FirstName, MiddleName, Surname FROM Volunteers");
				var table = connector.ExecuteDataTable();
				connector = null;
				
				var count = 0;
				foreach(DataRow row in table.Rows)
				{
					count++;
					outputLine("Record #" + count.ToString());
					string outputRecord = "";
					foreach(DataColumn col in table.Columns)
					{
						if(outputRecord != "") outputRecord += ", ";
						outputRecord += col.ColumnName + ": " + row[col.ColumnName].ToString();
					}
					outputLine(outputRecord);
				}
			}
			catch (Exception ex)
			{
				outputWarning("Error! " + ex.Message);
				outputLine("Exception type: " + ex.GetType().ToString());
				outputLine("Stack trace: " + ex.StackTrace);
			}
		}
		
		public static void InsertTest(Action<string> outputLine, Action<string> outputWarning)
		{
			try
			{
				outputLine("Attempting INSERT");
				var connector = new Connector("INSERT INTO Volunteers (FirstName, MiddleName, Surname) VALUES (@FirstName, @MiddleName, @Surname)");
				connector.AddParameter("@FirstName", "John");
				connector.AddParameter("@MiddleName", "Test");
				connector.AddParameter("@Surname", "X");
				connector.ExecuteNonQuery();
				connector = null;
				outputLine("INSERT Completed!");
			}
			catch (Exception ex)
			{
				outputWarning("Error! " + ex.Message);
				outputLine("Exception type: " + ex.GetType().ToString());
				outputLine("Stack trace: " + ex.StackTrace);
			}
		}
		
		public void ExecuteNonQuery()
		{
			this.OpenConnection();
			this.Command.ExecuteNonQuery();
			this.CloseConnection();
		}
		
		public DataTable ExecuteDataTable()
		{
			this.OpenConnection();
			
			var adapter = new OleDbDataAdapter();
			adapter.SelectCommand = this.Command;
			
			var table = new DataTable();
			adapter.Fill(table);
			
			this.CloseConnection();
			
			return table;
		}
		
		public void AddParameter(string paramName, object paramValue)
		{
			this.Command.Parameters.AddWithValue(paramName, paramValue);
		}
		
		public void OpenConnection()
		{
			if(this.Connection.State == ConnectionState.Closed)
				this.Connection.Open();
		}
		
		public void CloseConnection()
		{
			if(this.Connection.State != ConnectionState.Closed)
				this.Connection.Close();
		}
		
	}
}
