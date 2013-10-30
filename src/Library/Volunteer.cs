
using System;
using System.Collections.Generic;
using RbcTools.Library.Database;

namespace RbcTools.Library
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
				         " CongPrivilege, RegularPioneer, NameOfMate, " +
				         " Trade1, Experience1, Years1, " +
				         " Trade2, Experience2, Years2, " +
				         " Trade3, Experience3, Years3, " +
				         " Trade4, Experience4, Years4," +
				         " EmergencyContactName, EmergencyContactRelationship," +
				         " EmergencyContactPhoneNo, EmergencyContactAddress) " +
				         "VALUES (@ApplicationKind, @HallConstruction, @DisasterRelief, " +
				         " @Surname, @FirstName, @MiddleName, @Gender, @DateOfBirth, @DateOfBaptism, " +
				         " @Address, @EmailAddress, @PhoneNumberHome, @PhoneNumberWork, @PhoneNumberMobile, " +
				         " @CongPrivilege, @RegularPioneer, @NameOfMate, " +
				         " @Trade1, @Experience1, @Years1, " +
				         " @Trade2, @Experience2, @Years2, " +
				         " @Trade3, @Experience3, @Years3, " +
				         " @Trade4, @Experience4, @Years4," +
				         " @EmergencyContactName, @EmergencyContactRelationship, " +
				         " @EmergencyContactPhoneNumber, @EmergencyContactAddress) ");
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
				         " EMailAddress = @EmailAddress, " +
				         " HomePhoneNo = @PhoneNumberHome, " +
				         " WorkPhoneNo = @PhoneNumberWork, " +
				         " MobilePhoneNo = @PhoneNumberMobile, " +
				         " CongPrivilege = @CongPrivilege, " +
				         " RegularPioneer = @RegularPioneer, " +
				         " NameOfMate = @NameOfMate, " +
				         " Trade1 = @Trade1, Experience1 = @Experience1, Years1 = @Years1, " +
				         " Trade2 = @Trade2, " +
				         " Experience2 = @Experience2, " +
				         " Years2 = @Years2, " +
				         " Trade3 = @Trade3, " +
				         " Experience3 = @Experience3, " +
				         " Years3 = @Years3, " +
				         " Trade4 = @Trade4, " +
				         " Experience4 = @Experience4, " +
				         " Years4 = @Years4, " +
				         " EmergencyContactName = @EmergencyContactName, " +
				         " EmergencyContactRelationship = @EmergencyContactRelationship, " +
				         " EmergencyContactPhoneNo = @EmergencyContactPhoneNumber, " +
				         " EmergencyContactAddress = @EmergencyContactAddress " +
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
			connector.AddParameter("@NameOfMate", this.NameOfMate);
			
			connector.AddParameter("@Trade1", this.WorkBackgroundList[0].TradeOrProfession);
			connector.AddParameter("@Experience1", this.WorkBackgroundList[0].TypeOfExprience);
			connector.AddParameter("@Years1", this.WorkBackgroundList[0].Years);
			
			connector.AddParameter("@Trade2", this.WorkBackgroundList[1].TradeOrProfession);
			connector.AddParameter("@Experience2", this.WorkBackgroundList[1].TypeOfExprience);
			connector.AddParameter("@Years2", this.WorkBackgroundList[1].Years);
			
			connector.AddParameter("@Trade3", this.WorkBackgroundList[2].TradeOrProfession);
			connector.AddParameter("@Experience3", this.WorkBackgroundList[2].TypeOfExprience);
			connector.AddParameter("@Years3", this.WorkBackgroundList[2].Years);
			
			connector.AddParameter("@Trade4", this.WorkBackgroundList[3].TradeOrProfession);
			connector.AddParameter("@Experience4", this.WorkBackgroundList[3].TypeOfExprience);
			connector.AddParameter("@Years4", this.WorkBackgroundList[3].Years);
			
			connector.AddParameter("@EmergencyContactName", this.EmergencyContactName);
			connector.AddParameter("@EmergencyContactRelationship", this.EmergencyContactRelationship);
			connector.AddParameter("@EmergencyContactPhoneNumber", this.EmergencyContactPhoneNumber);
			connector.AddParameter("@EmergencyContactAddress", this.EmergencyContactAddress);
			
			connector.AddParameter("@ID", this.ID);
			connector.ExecuteNonQuery();
			connector = null;
		}
		
		#endregion
	}
}