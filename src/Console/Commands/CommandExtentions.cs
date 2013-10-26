using System;
using System.Collections.Generic;
using System.Linq;

namespace RbcConsole.Commands
{
	public static class CommandExtentions
	{
		public static List<string> ListSlugs(this List<CommandBase> source)
		{
			var query = from command in source
				select command.Slug;
			return query.ToList();
		}
	}
}
