
using System;
using System.Collections.Generic;
using RbcVolunteerApplications.Library.Database;

namespace RbcVolunteerApplications.Library
{
	/// <summary>
	///  A representation of the RBC Volunteer Application form.
	///  Based on S-82-E Bi 5/12
	/// </summary>
	public class Volunteer
	{
		
		public Volunteer(){}
		
		#region Properties
		
		public int ID { get; set; }
		
		public ApplicationKind ApplicationKind { get; set; }
		
		public FormOfServiceKinds FormsOfService { get; set; }
		
		public string LastName { get; set; }
		
		public string FirstName { get; set; }
		
		public string MiddleNames { get; set; }
		
		public GenderKind Gender { get; set; }
		
		public DateTime DateOfBirth { get; set; }
		
		public DateTime DateOfBaptism { get; set; }
		
		public string Address { get; set; }
		
		public string EmailAddress { get; set; }
		
		public string PhoneNumberHome { get; set; }
		
		public string PhoneNumberWork { get; set; }
		
		public string PhoneNumberMobile { get; set; }
		
		public CongregationPrivilegeKinds CongregationPrivileges { get; set; }
		
		public bool RegularPioneer { get; set; }
		
		public string NameOfMate { get; set; }
		
		public string BackgroundTradeOrProfession1 { get; set; }
		
		public List<WorkBackground> WorkBackgroundList { get; set; }
		
		public string EmergencyContactName { get; set; }
		
		public string EmergencyContactRelationship { get; set; }
		
		public string EmergencyContactPhoneNumber { get; set; }
		
		public string EmergencyContactAddress { get; set; }
		
		#endregion
		
		#region Methods
		
		public void SaveToDatabase()
		{
			var query = "";
			if(this.ID == 0)
			{
				// INSERT into database
				query = ("INSERT INTO Volunteers " +
				         "(TypeOfApplication, KingdomAssemblyHallConstruction, DisasterRelief, " +
				         " Surname, FirstName, MiddleName, Gender, DateOfBirth, DateOfBaptism, " +
				         " Address, EMailAddress, HomePhoneNo, WorkPhoneNo, MobilePhoneNo, " +
				         " CongPrivilege, RegularPioneer) " +
				         "VALUES (@ApplicationKind, @HallConstruction, @DisasterRelief, " +
				         " @Surname, @FirstName, @MiddleName, @Gender, @DateOfBirth, @DateOfBaptism, " +
				         " @Address, @EmailAddress, @PhoneNumberHome, @PhoneNumberWork, @PhoneNumberMobile, " +
				         " @CongPrivilege, @RegularPioneer) ");
			}
			else
			{
				// UPDATE database
				query = ("UPDATE Volunteers SET " +
				         " TypeOfApplication = @ApplicationKind, " +
				         " KingdomAssemblyHallConstruction = @HallConstruction," +
				         " DisasterRelief = @DisasterRelief," +
				         " Surname = @Surname, " +
				         " FirstName = @FirstName, " +
				         " MiddleName = @MiddleName, " +
				         " Gender = @Gender, " +
				         " DateOfBirth = @DateOfBirth, " +
				         " DateOfBaptism = @DateOfBaptism, " +
				         " Address = @Address," +
				         " EMailAddress = @EmailAddress," +
				         " HomePhoneNo = @PhoneNumberHome," +
				         " WorkPhoneNo = @PhoneNumberWork," +
				         " MobilePhoneNo = @PhoneNumberMobile," +
				         " CongPrivilege = @CongPrivilege," +
				         " RegularPioneer = @RegularPioneer" +
				         " WHERE ID = @ID");
			}
			
			var connector = new Connector(query);
			connector.AddParameter("@ApplicationKind", this.ApplicationKind.GetName());
			connector.AddParameter("@HallConstruction", this.FormsOfService.HasFlag(FormOfServiceKinds.HallConstruction));
			connector.AddParameter("@DisasterRelief", this.FormsOfService.HasFlag(FormOfServiceKinds.DisasterRelief));
			connector.AddParameter("@Surname", "AAA DONT DELETE " + this.LastName);
			connector.AddParameter("@FirstName", this.FirstName);
			connector.AddParameter("@MiddleName", this.MiddleNames);
			connector.AddParameter("@Gender", this.Gender.ToString());
			connector.AddParameter("@DateOfBirth", this.DateOfBirth);
			connector.AddParameter("@DateOfBaptism", this.DateOfBaptism);
			connector.AddParameter("@Address", this.Address);
			connector.AddParameter("@EmailAddress", this.EmailAddress);
			connector.AddParameter("@PhoneNumberHome", this.PhoneNumberHome);
			connector.AddParameter("@PhoneNumberWork", this.PhoneNumberWork);
			connector.AddParameter("@PhoneNumberMobile", this.PhoneNumberMobile);
			connector.AddParameter("@CongPrivilege", this.CongregationPrivileges.GetName());
			connector.AddParameter("@RegularPioneer", this.RegularPioneer);
			connector.AddParameter("@ID", this.ID);
			connector.ExecuteNonQuery();
			connector = null;
		}
		
		#endregion
	}
}