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
			this.Connection = new OleDbConnection(this.ConnectionString);
			this.Command = new OleDbCommand(query, this.Connection);
		}
		
		#endregion
		
		#region Fields
		
		private string _FilePath = null;
		private string _ConnectionString = null;
		private string _Query;
		private OleDbConnection Connection;
		private OleDbCommand Command;
		
		#endregion
		
		#region Properties
		
		private string AccessFilePath
		{
			get
			{
				if(_FilePath == null)
				{
					var exeDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
					_FilePath = exeDirectory + @"\RBCWalesVolsDatabase.accdb";
				}
				return _FilePath;
			}
		}
		
		private string ConnectionString
		{
			get
			{
				if(_ConnectionString == null)
				{
					if(this.AccessFileExists)
						_ConnectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}", this.AccessFilePath);
					else
						throw new AccessFileMissingException("The required Access database file was missing: " + this.AccessFilePath);
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
		
		public bool AccessFileExists
		{
			get
			{
				return File.Exists(this.AccessFilePath);
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
