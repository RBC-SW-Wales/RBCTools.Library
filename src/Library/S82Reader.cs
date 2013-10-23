using System;
using System.Collections.Generic;
using System.Linq;

using iTextSharp.text.pdf;

namespace RbcVolunteerApplications.Library
{
	public class S82Reader
	{
		
		#region Constructor 
		
		public S82Reader(string filePath)
		{
			this.FilePath = filePath;
			
			var reader = new PdfReader(this.FilePath);
			this.AcroFields = reader.AcroFields;
			reader = null;
		}
		
		#endregion
		
		#region Fields
		
		private AcroFields AcroFields;
		
		#endregion
		
		#region Properties
		
		public string FilePath { get; set; }
		
		public string this[string fieldName]
		{
			get
			{
				return GetPdfValue(fieldName);
			}
		}
		
		public List<string> Keys
		{
			get
			{
				var query = (from entry in this.AcroFields.Fields
				             select entry.Key);
				
				return query.ToList();
			}
		}
		
		#endregion
		
		private string GetPdfValue(string key)
		{
			return this.AcroFields.GetField(key).Trim().TrimInnerWhitespace();
		}
		
		public bool GetCheckBoxValue(string key)
		{
			return GetPdfValue(key) == "Yes";
		}
		
	}
}
