using System;

namespace RbcTools.Library.Database
{
	public class AccessFileMissingException : Exception
	{
		public AccessFileMissingException(){}
		
		public AccessFileMissingException(string message):base(message) {}
	}
}
