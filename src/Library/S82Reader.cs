using System;
using System.Collections.Generic;

namespace RbcVolunteerApplications.Library
{
	public class S82Reader
	{
		public S82Reader(string filePath)
		{
			this.ReadS82File(filePath);
		}
		
		public bool ReadSuccessful { get; set; }
		
		public List<string> ProblemList { get; set; }
		
		public VolunteerApplication VolunteerApplication { get; set; }
		
		private void ReadS82File(string filePath)
		{
			this.ReadSuccessful = true;
			this.ProblemList = new List<string>();
			
			// When there is a problem, do this:
			this.ReadSuccessful = false;
			this.ProblemList.Add("Could not read BLA field. Please correct and try again.");
		}
	}
}
