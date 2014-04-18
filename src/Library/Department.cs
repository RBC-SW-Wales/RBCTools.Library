using System;
using System.Data;

namespace RbcTools.Library
{
	public class Department
	{
		public Department(){}
		public Department(DataRow row)
		{
			this.ID = (int)row["ID"];
			this.Name = (string)row["Trade"];
		}
		
		public int ID { get; set; }
		public string Name { get; set; }
	}
}
