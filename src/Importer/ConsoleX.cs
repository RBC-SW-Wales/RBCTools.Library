using System;

namespace RbcVolunteerApplications.Importer
{
	public static class ConsoleX
	{
		public static void WriteLine(string text)
		{
			Console.WriteLine("    " + text);
			Console.WriteLine(" ");
		}
		
		public static void WriteWarning(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			ConsoleX.WriteLine("    " + text);
			Console.ResetColor();
		}
		
		public static string ReadPromt()
		{
			var input = "";
			Console.Write(" >> ");
			input = Console.ReadLine();
			Console.WriteLine(" ");
			return input;
		}
		
		public static string WriteQuery(string text)
		{
			ConsoleX.WriteLine(text);
			var input =  ConsoleX.ReadPromt();
			ConsoleX.WriteLine("Thanks.");
			return input;
		}
		
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
		
	}
}
