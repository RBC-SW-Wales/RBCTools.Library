using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using RbcTools.Library;
using RbcTools.Library.Database;

namespace RbcConsole.Helpers
{
	public static class AccessFileHelper
	{
		
		private static string CorrectHash = "76dfc75952b6fb67da4dde043056a61190d8001a4a149816b64c17f0b244552e";
		
		public static void CheckForAccessFile(IConsoleX consoleX)
		{
			if(!Connector.AccessFileExists)
			{
				string url = "";
				string hash = "";
				
				consoleX.WriteWarning("Looks like you don't have the correct Access database file in place.");
				consoleX.WriteLine("Don't worry though, I can download it for you and put it into the correct place.");
				
				do
				{
					url = consoleX.WriteQuery("What is the CORRECT URL for the Access database file?");
					hash = GetUrlHash(url);
					
//					consoleX.WriteLine(hash);
					
					if(hash != CorrectHash)
						consoleX.WriteLine("Sorry, incorrect URL. Please try again.");
				}
				while(hash != CorrectHash);
				
				consoleX.WriteLine("Downloading file...");
				var client = new WebClient();
				client.DownloadFile(url, Connector.AccessFilePath);
				consoleX.WriteLine("Done.");
			}
		}
		
		private static string GetUrlHash(string url)
		{
			var sha256 = SHA256.Create();
			var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(url));
			StringBuilder builder = new StringBuilder(bytes.Length * 2);
			foreach(var b in bytes)
			{
				builder.Append(b.ToString("x2"));
			}
			return builder.ToString();
		}
	}
}
