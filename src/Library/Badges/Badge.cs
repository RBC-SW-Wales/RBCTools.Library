
using System;
using System.Data;

namespace RbcTools.Library.Badges
{
	public class Badge
	{
		#region Constructors
		
		public Badge(){}
		
		public Badge(DataRow row)
		{
			this.FirstName = row["FirstName"] as string;
			this.LastName = row["Surname"] as string;
			this.CongregationName = row["Congregation"] as string;
			this.DepartmentName = row["Trade"] as string;
			this.HasDrillsTraining = (bool)row["Drills"];
		}
		
		#endregion
		
		
		
		public string FirstName { get; set; }
		
		public string LastName { get; set; }
		
		public string FullName
		{
			get
			{
				return FirstName + " " + LastName;
			}
		}
		
		public string CongregationName { get; set; }
		
		public string DepartmentName { get; set; }
		
		public bool HasDrillsTraining { get; set; }
		
		public bool HasPlanersTraing { get; set; }
		
		public bool HasRoutersTraining { get; set; }
		
		public bool HasCitbPlantTraining { get; set; }
		
		public bool HasJigsawsTraining { get; set; }
		
		public bool HasNailersTraining { get; set; }
		
		public bool HasChopSawsTraining { get; set; }
		
		public bool HasCircularSawsTraining { get; set; }
		
		public bool HasRoofAndScaffoldAccess { get; set; }
		
		public bool HasSiteAccess { get; set; }
		
	}
}
