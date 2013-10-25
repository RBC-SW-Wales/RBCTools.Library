using System;
using System.Collections.Generic;

namespace RbcVolunteerApplications.Library
{
	[Flags]
	public enum CongregationPrivilegeKinds
	{
		NoneSpecified = 0,
		Elder = 1,
		MinisterialServant = 2
	}
	
	public static class CongregationPrivilegeExtentions
	{
		public static string GetName(this CongregationPrivilegeKinds source)
		{
			if(source.HasFlag(CongregationPrivilegeKinds.Elder))
				return "Elder";
			else if(source.HasFlag(CongregationPrivilegeKinds.MinisterialServant))
				return "Ministerial Servant";
			else
				return "";
		}
	}
}
