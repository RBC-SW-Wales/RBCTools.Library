using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RbcVolunteerApplications.Library;

namespace RbcVolunteerApplications.Importer
{
	public class ConsoleX : IConsoleX
	{
		public ConsoleX(){}
		
		public void WriteLine(string text){ this.WriteLine(text, blankAfter: true); }
		
		public void WriteLine(string text, bool blankAfter = true)
		{
			Console.WriteLine("    " + text);
			if(blankAfter) Console.WriteLine(" ");
		}
		
		public void WriteWarning(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			this.WriteLine(text);
			Console.ResetColor();
		}
		
		public void WriteLine(string text, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			this.WriteLine(text);
			Console.ResetColor();
		}
		
		public string ReadPromt(List<string> tabPossibilities = null)
		{
			var input = string.Empty;
			Console.Write(" >> ");
			
			// If no tab possibilies use normal ReadLine(), otherwise use special handling.
			if(tabPossibilities == null || tabPossibilities.Count == 0)
				input = Console.ReadLine();
			else
				input = ReadInputWithTabCompletion(input, tabPossibilities);
			
			Console.WriteLine(" ");
			return input;
		}

		private string ReadInputWithTabCompletion(string input, List<string> tabPossibilities)
		{
			ConsoleKeyInfo key;
			// Loop until ENTER key is hit
			do
			{
				key = Console.ReadKey(true);
				switch(key.Key)
				{
					case ConsoleKey.Backspace:
						// If there is input, remove last character.s
						if(input.Length > 0)
						{
							input = input.Substring(0, (input.Length - 1));
							Console.Write("\b \b");
						}
						break;
					case ConsoleKey.Tab:
						// Search possibilities for a single match.
						if(tabPossibilities != null)
						{
							// Get all possible matches
							var matches = tabPossibilities.FindAll(x => x.StartsWith(input));
							// If one unique found, use that.
							if(matches.Count == 1)
							{
								// Reset cursor back to start of 'input', then write out new value
								Console.SetCursorPosition(Console.CursorLeft - input.Length, Console.CursorTop);
								input = matches[0];
								Console.Write(input);
							}
						}
						break;
					case ConsoleKey.Enter:
						// Finish the line, loop will exit on this character.
						Console.WriteLine();
						break;
					default:
						// Build up input and write out the character to the screen.
						input += key.KeyChar.ToString();
						Console.Write(key.KeyChar);
						break;
				}
			} while (key.Key != ConsoleKey.Enter);
			
			return input;
		}
		
		public string WriteQuery(string text)
		{
			this.WriteLine(text);
			var input = this.ReadPromt();
			return input;
		}
		
		public void WriteHorizontalRule()
		{
			Console.Write(" "); // space
			Console.Write(new string('-', Console.WindowWidth - 2)); // width minus 2 spaces.
			Console.Write(" "); // space
			Console.WriteLine(" ");
		}
		
		private void WriteHeading(string text, ConsoleColor color)
		{
			Console.WriteLine(" ");
			Console.WriteLine(" ");
			Console.ForegroundColor = color;
			this.WriteLine(text, blankAfter:false);
			this.WriteHorizontalRule();
			Console.ResetColor();
		}
		
		public void WriteTitle(string text)
		{
			this.WriteHeading(text, ConsoleColor.Green);
		}
		
		public void WriteIntro(string text)
		{
			this.WriteHeading(text, ConsoleColor.Blue);
		}
		
		public void WriteException(Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			this.WriteLine("Error: " + ex.Message);
			this.WriteLine("Type: " + ex.GetType().ToString());
			this.WriteLine("Stack Trace: " + ex.StackTrace);
			Console.ResetColor();
		}
		
		public void WriteDataTable(DataTable table, int columnWidth = 20)
		{
			if(table != null)
			{
				var tableWidth = (table.Columns.Count * (columnWidth + 3)) + 1;
				var cellFormat = "| {0,-" + columnWidth + "} ";
				
				this.WriteLine(new string('-', tableWidth), blankAfter:false);
				
				string line = "";
				foreach(DataColumn column in table.Columns)
				{
					line += string.Format(cellFormat, column.ColumnName.TruncateIfTooLong(columnWidth, "..."));
				}
				line += "|";
				
				this.WriteLine(line, blankAfter:false);
				
				this.WriteLine(new string('-', tableWidth), blankAfter:false);
				
				foreach(DataRow row in table.Rows)
				{
					line = "";
					foreach(DataColumn col in table.Columns)
					{
						line += string.Format(cellFormat, row[col.ColumnName].ToString().TruncateIfTooLong(columnWidth, "..."));
					}
					line += "|";
					this.WriteLine(line, blankAfter:false);
				}
				
				this.WriteLine(new string('-', tableWidth), blankAfter: false);
				
				this.WriteLine(string.Format("Record count: {0}", table.Rows.Count));
				
			}
		}
	}
}
