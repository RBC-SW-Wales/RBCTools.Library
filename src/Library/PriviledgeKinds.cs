using System;
using System.Collections.Generic;

namespace RbcVolunteerApplications.Library
{
	[Flags]
	public enum PriviledgeKinds
	{
		NoneSpecified = 0,
		Elder = 1,
		MinisterialServant = 2,
		RegularPioneer = 4
	}
}
