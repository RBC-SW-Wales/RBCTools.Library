using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RbcVolunteerApplications.Library;

namespace RbcVolunteerApplications.Library
{
	public interface IConsoleX
	{
		void WriteLine(string text);
		void WriteLine(string text, bool blankAfter);
		void WriteWarning(string text);
		void WriteLine(string text, ConsoleColor color);
		string ReadPromt(List<string> tabPossibilities = null);
		string WriteQuery(string text);
		void WriteHorizontalRule();
		void WriteTitle(string text);
		void WriteIntro(string text);
		void WriteException(Exception ex);
		void WriteDataTable(DataTable table, int columnWidth = 20);
	}
}
