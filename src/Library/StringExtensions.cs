
using System;

namespace RbcVolunteerApplications.Library
{
	/// <summary>
	/// Extension methods for string
	/// </summary>
	public static class StringExtensions
	{
		public static string TrimInnerWhitespace(this string source)
		{
			while(source.Contains("  "))
				source = source.Replace("  ", " ");
			
			return source;
		}
		
		public static string TruncateIfTooLong(this string source, int maxLength, string trailingString = "")
		{
			if(source.Length > maxLength)
				source = source.Substring(0, maxLength - trailingString.Length) + trailingString;
			
			return source;
		}
	}
}
