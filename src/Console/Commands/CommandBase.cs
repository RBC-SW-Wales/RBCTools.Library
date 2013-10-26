
using System;
using RbcConsole.Helpers;

namespace RbcConsole.Commands
{
	public abstract class CommandBase
	{
		public CommandBase(){}
		
		public ConsoleX ConsoleX = new ConsoleX();
		
		public string Slug { get; set; }
		
		public string Description { get; set; }
		
		public bool IsDatabaseCommand { get; set; }
		
		public bool SkipIntroAndRule { get; set; }
		
		public void Execute()
		{
			if(this.IsDatabaseCommand)
				AccessFileHelper.CheckForAccessFile(this.ConsoleX);
			
			if(!this.SkipIntroAndRule)
				ConsoleX.WriteIntro(this.Description);
			
			this.Run();
			
			if(!this.SkipIntroAndRule)
				ConsoleX.WriteHorizontalRule();
		}
		
		public abstract void Run();
	}
}
