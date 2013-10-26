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
				
				consoleX.WriteIntro("Database requirements");
				
				string url = "";
				string hash = "";
				
				consoleX.WriteWarning("It looks like you don't have the Access database file yet.");
				consoleX.WriteLine("Don't worry, I can download it for you and save it to the correct place.");
				
				do
				{
					url = consoleX.WriteQuery("What is the CORRECT URL for the Access database file?");
					hash = GetUrlHash(url);
					
					// consoleX.WriteLine(hash);
					
					if(hash != CorrectHash)
						consoleX.WriteLine("Sorry, incorrect URL. Please try again.");
				}
				while(hash != CorrectHash);
				
				consoleX.WriteLine("Thanks! Downloading file now. Please wait...");
				
				using(var client = new WebClient())
				{
					client.DownloadFile(url, Connector.AccessFilePath);
				}
				
				consoleX.WriteLine("OK, Done. You can now continue with your previous task.");
				consoleX.WriteHorizontalRule();
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
