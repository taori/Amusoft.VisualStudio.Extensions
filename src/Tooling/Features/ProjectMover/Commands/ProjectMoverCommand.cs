using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Tooling.Features.ProjectMover.Views;
using Tooling.Shared.Helpers;
using Tooling.Shared.Resources;
using Tooling.Utility;
using Task = System.Threading.Tasks.Task;

namespace Tooling.Features.ProjectMover.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class ProjectMoverCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 4129;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("b06e81b0-3ef8-4c81-b260-412cd82b7366");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly AsyncPackage package;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectMoverCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		/// <param name="commandService">Command service to add command to, not null.</param>
		private ProjectMoverCommand(AsyncPackage package, OleMenuCommandService commandService)
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
				command.Text = Translations.ProjectMoverToolWindowTitle;
			}
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static ProjectMoverCommand Instance
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
			// Switch to the main thread - the call to AddCommand in OpenProjectMoverCommandCommand's constructor requires
			// the UI thread.
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			Instance = new ProjectMoverCommand(package, commandService);
		}

		/// <summary>
		/// Shows the tool window when the menu item is clicked.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		private void Execute(object sender, EventArgs e)
		{
			ToolWindowHelper.Prompt<ProjectMoverToolWindow>(this.package);
		}
	}
}
