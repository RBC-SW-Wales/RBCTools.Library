
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
	}
}
