
using System;
using System.Data;

namespace RbcTools.Library.Badges
{
	public class Badge
	{
		#region Constructors
		
		public Badge()
		{
			this.FirstName = "";
			this.MiddleName = "";
			this.LastName = "";
			this.CongregationName = "";
			this.DepartmentName = "";
			this.HasDrillsTraining = false;
			this.HasPlanersTraing = false;
			this.HasRoutersTraining = false;
			this.HasCitbPlantTraining  = false;
			this.HasJigsawsTraining = false;
			this.HasNailersTraining = false;
			this.HasChopSawsTraining = false;
			this.HasCircularSawsTraining = false;
			this.HasRoofAndScaffoldAccess = false;
			this.HasSiteAccess = false;
		}
		
		public Badge(DataRow row)
		{
			this.VolunteerID = (int)row["ID"];
			this.FirstName = row["FirstName"] as string;
			this.MiddleName = row["MiddleName"] as string;
			this.LastName = row["Surname"] as string;
			this.CongregationName = row["Congregation"] as string;
			this.DepartmentName = row["TradeName"] as string;
			this.HasDrillsTraining = (bool)row["Drills"];
			this.HasPlanersTraing = (bool)row["Planers"];
			this.HasRoutersTraining = (bool)row["Routers"];
			this.HasCitbPlantTraining = (bool)row["CITBPlant"];
			this.HasJigsawsTraining = (bool)row["Jigsaw"];
			this.HasNailersTraining = (bool)row["Nailers"];
			this.HasChopSawsTraining = (bool)row["ChopSaw"];
			this.HasCircularSawsTraining = (bool)row["CircSaw"];
			this.HasRoofAndScaffoldAccess = (bool)row["RoofScaffold"];
			this.HasSiteAccess = (bool)row["Inducted"];
		}
		
		#endregion
		
		#region Properties
		
		public int VolunteerID { get; set; }
		
		public string FirstName { get; set; }
		
		public string MiddleName { get; set; }
		
		public string LastName { get; set; }
		
		public string FullName
		{
			get
			{
				var fullName = "";
				
				if(!string.IsNullOrWhiteSpace(this.MiddleName))
					fullName = string.Format("{0} {1} {2}", this.FirstName, this.MiddleName, this.LastName);
				else
					fullName = string.Format("{0} {1}", this.FirstName, this.LastName);
				
				return fullName;
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
		
		#endregion
	}
}
