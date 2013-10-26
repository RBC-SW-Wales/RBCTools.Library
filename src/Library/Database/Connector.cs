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
