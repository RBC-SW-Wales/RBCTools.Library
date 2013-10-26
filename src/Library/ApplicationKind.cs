using System;
using System.Collections.Generic;

namespace RbcTools.Library
{
	public enum ApplicationKind
	{
		NoneSpecified = 0,
		NewApplication = 1,
		UpdateOfPersonalData = 2
	}
	
	public static class ApplicationKindExtentions
	{
		public static string GetName(this ApplicationKind kind)
		{
			string name = "";
			switch(kind)
			{
				case ApplicationKind.NewApplication:
					name = "New Application";
					break;
				case ApplicationKind.UpdateOfPersonalData:
					name = "Update of Personal Data";
					break;
			}
			return name;
		}
	}
}
