
using System;
using System.Collections.Generic;
using RbcVolunteerApplications.Library.Database;

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
		
		public FormOfServiceKinds FormsOfService { get; set; }
		
		public string LastName { get; set; }
		
		public string FirstName { get; set; }
		
		public string MiddleNames { get; set; }
		
		public GenderKind Gender { get; set; }
		
		public DateTime DateOfBirth { get; set; }
		
		public DateTime DateOfBaptism { get; set; }
		
		public string AddressStreetAndNumber { get; set; }
		
		public string AddressTown { get; set; }
		
		public string PostCode { get; set; }
		
		public string EmailAddress { get; set; }
		
		public string PhoneNumberHome { get; set; }
		
		public string PhoneNumberWork { get; set; }
		
		public string PhoneNumberMobile { get; set; }
		
		public PriviledgeKinds CurrentPriviledges { get; set; }
		
		public string NameOfMate { get; set; }
		
		public string BackgroundTradeOrProfession1 { get; set; }
		
		public List<WorkBackground> WorkBackgroundList { get; set; }
		
		public string EmergencyContactName { get; set; }
		
		public string EmergencyContactRelationship { get; set; }
		
		public string EmergencyContactPhoneNumber { get; set; }
		
		public string EmergencyContactAddress { get; set; }
		
		#endregion
		
		#region Methods
		
		public void InsertIntoDatabase()
		{
			var connector = new Connector("INSERT INTO Volunteers " + 
			                              "(Surname, FirstName, MiddleName) " +
			                              "VALUES (@Surname, @FirstName, @MiddleName)");
			
			connector.AddParameter("@Surname", "AAA TEST " + this.LastName);
			connector.AddParameter("@FirstName", this.FirstName);
			connector.AddParameter("@MiddleName", this.MiddleNames);
			connector.ExecuteNonQuery();
			connector = null;
		}
		
		#endregion
	}
}