using System;
using RbcTools.Library.Database;

namespace RbcConsole.Commands
{
	public class ListCongregations : CommandBase
	{
		public ListCongregations()
		{
			base.Slug = "list-congregations";
			base.Description = "Show all a table of all Congregations (ID, Number and Name)";
			base.IsDatabaseCommand = true;
		}
		
		public override void Run()
		{
			ConsoleX.WriteDataTable(Congregations.GetAll(), 20);
		}
	}
}
