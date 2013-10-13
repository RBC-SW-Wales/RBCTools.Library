using System;

namespace RbcVolunteerApplications.Importer
{
	public static class ConsoleX
	{
		public static void WriteTitle(string intro)
		{
			Console.WriteLine(" ");
			Console.WriteLine(" ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("    " + intro);
			Console.Write(new string('-', Console.WindowWidth));
			Console.WriteLine(" ");
			Console.ResetColor();
		}
		
		public static void WriteIntro(string intro)
		{
			Console.WriteLine(" ");
			Console.WriteLine(" ");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("    " + intro);
			Console.Write(new string('-', Console.WindowWidth));
			Console.WriteLine(" ");
			Console.ResetColor();
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
		
		public static string WriteQuery(string text)
		{
			Console.WriteLine("    " + text);
			Console.WriteLine(" ");
			var input =  Console.ReadLine();
			Console.WriteLine(" ");
			Console.WriteLine("Thanks.");
			Console.WriteLine(" ");
			return input;
		}
		
	}
}
