
using System;
using System.Collections.Generic;

namespace RbcVolunteerApplications.Library
{
	/// <summary>
	///  A representation of the RBC Volunteer Application form.
	///  Based on S-82-E Bi 5/12
	/// </summary>
	public class VolunteerApplication
	{
		
		public VolunteerApplication(){}
		
		#region Properties
		
		public ApplicationKind ApplicationKind { get; set; }
		
		public FormsOfService FormsOfService { get; set; }
		
		public string LastName { get; set; }
		
		public string FirstName { get; set; }
		
		public string MiddleName { get; set; }
		
		public Gender Gender { get; set; }
		
		public DateTime DateOfBirth { get; set; }
		
		public DateTime DateOfBaptism { get; set; }
		
		#endregion
		
	}
	
	public enum ApplicationKind
	{
		NoneSpecified = 0,
		NewApplication = 1,
		UpdateOfPersonalData = 2
	}
	
	[Flags]
	public enum FormsOfService
	{
		NoneSpecified = 0,
		HallConstruction = 1,
		DisasterRelief = 2
	}
	
	public enum Gender
	{
		NoneSpecified = 0,
		Male = 1,
		Femail = 2
	}
}