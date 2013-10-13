using System;

namespace RbcVolunteerApplications.Importer
{
	public static class ConsoleX
	{
		public static void WriteIntro(string intro)
		{
			Console.WriteLine(" ");
			Console.Write(new string('-', Console.WindowWidth));
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("    " + intro);
			Console.ResetColor();
			Console.Write(new string('-', Console.WindowWidth));
			Console.WriteLine(" ");
		}
		
		public static void WriteLine(string text)
		{
			Console.WriteLine("    " + text);
			Console.WriteLine(" ");
		}
		
		public static void WriteWarning(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("    " + text);
			Console.WriteLine(" ");
			Console.ResetColor();
		}
		
	}
}
