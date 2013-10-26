using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;

namespace RbcTools.Library.Database
{
	public class Connector
	{
		#region Contructors
		
		public Connector(string query)
		{
			this._Query = query;
			this.Connection = new OleDbConnection(Connector.ConnectionString);
			this.Command = new OleDbCommand(query, this.Connection);
		}
		
		#endregion
		
		#region Fields
		
		private static string _AccessFilePath = null;
		private static string _ConnectionString = null;
		private string _Query;
		private OleDbConnection Connection;
		private OleDbCommand Command;
		
		#endregion
		
		#region Properties
		
		public static string AccessFilePath
		{
			get
			{
				if(_AccessFilePath == null)
				{
					var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\RBCTool";
					
					if(!Directory.Exists(directory))
						Directory.CreateDirectory(directory);
					
					_AccessFilePath = directory + @"\RBCWalesVolsDatabase.accdb";
				}
				return _AccessFilePath;
			}
		}
		
		private static string ConnectionString
		{
			get
			{
				if(_ConnectionString == null)
				{
					if(Connector.AccessFileExists)
						_ConnectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}", Connector.AccessFilePath);
					else
						throw new AccessFileMissingException("The required Access database file was missing: " + Connector.AccessFilePath);
				}
				return _ConnectionString;
			}
		}
		
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
		
		public static bool AccessFileExists
		{
			get
			{
				return File.Exists(Connector.AccessFilePath);
			}
		}
		
		#endregion
		
		public void ExecuteNonQuery()
		{
			this.OpenConnection();
			this.Command.ExecuteNonQuery();
			this.CloseConnection();
		}
		
		public DataTable ExecuteDataTable()
		{
			//Console.WriteLine(this.Query);
			
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
