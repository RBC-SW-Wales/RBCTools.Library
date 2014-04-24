
using System;
using System.Runtime.Serialization;

namespace RbcTools.Library.Database
{
	public class AccessFileUrlException : ArgumentException
	{
		public AccessFileUrlException()
		{
		}

		public AccessFileUrlException(string message) : base(message)
		{
		}

		public AccessFileUrlException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}