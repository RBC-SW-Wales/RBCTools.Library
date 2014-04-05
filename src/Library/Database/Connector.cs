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
		
		private static string _ConnectionString = null;
		private string _Query;
		private OleDbConnection Connection;
		private OleDbCommand Command;
		
		#endregion
		
		#region Properties
		
		private static string ConnectionString
		{
			get
			{
				if(_ConnectionString == null)
				{
					if(AccessFileDownloader.AccessFileExists)
						_ConnectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}", AccessFileDownloader.AccessFilePath);
					else
						throw new AccessFileMissingException("The required Access database file was missing. File must be downloaded first to use this feature.");
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
		

		
		#endregion
		
		public void ExecuteNonQuery()
		{
			try
			{
				this.OpenConnection();
				this.Command.ExecuteNonQuery();
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				this.CloseConnection();
			}
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
