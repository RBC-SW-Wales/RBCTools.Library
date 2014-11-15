using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace RbcTools.Library.Database
{
	public static class AccessFileDownloader
	{
		#region Constants
		
		const string ACCESS_FILE_NAME = "Console_0001_RBCWalesVolsDatabase.accdb";
		const string ACCESS_URL_HASH = "76dfc75952b6fb67da4dde043056a61190d8001a4a149816b64c17f0b244552e";
		
		#endregion
		
		#region Fields
		
		private static string _AccessFilePath = null;
		
		#endregion
		
		#region Properties
		
		internal static string AccessFilePath
		{
			get
			{
				if(_AccessFilePath == null)
				{
					var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\RBCTool";
					
					if(!Directory.Exists(directory))
						Directory.CreateDirectory(directory);
					
					_AccessFilePath = directory + @"\" + ACCESS_FILE_NAME;
				}
				return _AccessFilePath;
			}
		}
		
		public static bool AccessFileExists
		{
			get
			{
				return File.Exists(AccessFilePath);
			}
		}
		
		#endregion
		
		#region Methods
		
		public static bool IsUrlCorrect(string url)
		{
			bool isUrlCorrect = false;
			
			string hash = GetUrlHash(url);
			
			if(hash == ACCESS_URL_HASH)
				isUrlCorrect = true;
			
			return isUrlCorrect;
		}
		
		public static void DownloadFile(string url)
		{
			if(IsUrlCorrect(url))
			{
				using(var client = new WebClient())
				{
					client.DownloadFile(url, AccessFilePath);
				}
			}
			else{ throw new AccessFileUrlException("The URL provided for the Access Database file was incorrect."); }
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
		
		public static void DeleteExistingFile()
		{
			if(AccessFileExists)
			{
				File.Delete(AccessFilePath);
			}
		}
		
		#endregion
	}
}
