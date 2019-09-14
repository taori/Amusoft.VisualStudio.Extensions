using System;
using System.ComponentModel.Design;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using ProjectTemplates.Resources;
using Task = System.Threading.Tasks.Task;

namespace ProjectTemplates.Infrastructure
{
	public class CommandRegistrar
	{
		public static void Initialize(IMenuCommandService commandService)
		{
//			var menuCommandID = new CommandID(PackageGuids.guidCompilerCmdSet, new CommandID(Constants.PackageIds.SolExpMenuGroup, ));
//			var menuItem = new OleMenuCommand(OnCreateCommand, menuCommandID);
//			menuItem.BeforeQueryStatus += BeforeQueryStatus;
//			commandService.AddCommand(menuItem);
		}

		private static void OnCreateCommand(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}