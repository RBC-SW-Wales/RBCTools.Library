using System;

namespace RbcVolunteerApplications.Importer
{
	public static class ConsoleX
	{
		
		public static void WriteLine(string text){ ConsoleX.WriteLine(text, blankAfter: true); }
		
		public static void WriteLine(string text, bool blankAfter = true)
		{
			Console.WriteLine("    " + text);
			if(blankAfter) Console.WriteLine(" ");
		}
		
		public static void WriteWarning(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			ConsoleX.WriteLine(text);
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
		
		private static void WriteHeading(string text, ConsoleColor color)
		{
			Console.WriteLine(" ");
			Console.WriteLine(" ");
			Console.ForegroundColor = color;
			ConsoleX.WriteLine(text, blankAfter:false);
			ConsoleX.WriteHorizontalRule();
			Console.ResetColor();
		}
		
		public static void WriteTitle(string text)
		{
			ConsoleX.WriteHeading(text, ConsoleColor.Green);
		}
		
		public static void WriteIntro(string text)
		{
			ConsoleX.WriteHeading(text, ConsoleColor.Blue);
		}
		
		public static void WriteHorizontalRule()
		{
			Console.Write(" "); // space
			Console.Write(new string('-', Console.WindowWidth - 2)); // width minus 2 spaces.
			Console.Write(" "); // space
			Console.WriteLine(" ");
		}
		
	}
}
