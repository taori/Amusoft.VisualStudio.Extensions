using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Tooling.Features.ProjectMover.Views;
using Tooling.Features.ProjectRenamer.ViewModels;
using Tooling.Features.ProjectRenamer.Views;
using Tooling.Shared.Helpers;
using Tooling.Shared.Resources;
using Tooling.Utility;
using Task = System.Threading.Tasks.Task;

namespace Tooling.Features.ProjectRenamer.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class RenameProjectCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 256;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("b06e81b0-3ef8-4c81-b260-412cd82b7366");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly AsyncPackage package;

		/// <summary>
		/// Initializes a new instance of the <see cref="RenameProjectCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		/// <param name="commandService">Command service to add command to, not null.</param>
		private RenameProjectCommand(AsyncPackage package, OleMenuCommandService commandService)
		{
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			var menuCommandID = new CommandID(CommandSet, CommandId);
			var menuItem = new OleMenuCommand(this.Execute, menuCommandID);
			menuItem.BeforeQueryStatus += MenuItemOnBeforeQueryStatus;
			commandService.AddCommand(menuItem);
		}

		private void MenuItemOnBeforeQueryStatus(object sender, EventArgs e)
		{
			if (sender is OleMenuCommand command)
			{
				command.Text = Translations.cmd_RenameProjectWithFolders;
			}
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static RenameProjectCommand Instance
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
		{
			get
			{
				return this.package;
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static async Task InitializeAsync(AsyncPackage package)
		{
			// Switch to the main thread - the call to AddCommand in RenameProjectCommand's constructor requires
			// the UI thread.
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			Instance = new RenameProjectCommand(package, commandService);
		}

		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void Execute(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			var project = SolutionHelper.GetCurrentProject();
			if (project == null)
			{
				LoggerHelper.Log("Unable to get current project.");
				return;
			}

			ToolWindowHelper.Prompt<ProjectRenameDialogToolPane>(this.package, new ProjectRenameDialogViewModel(project));
		}
	}
}
