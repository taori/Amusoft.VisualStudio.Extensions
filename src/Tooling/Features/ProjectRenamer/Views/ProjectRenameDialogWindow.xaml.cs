using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;
using Tooling.Features.ProjectMover.Views;
using Tooling.Shared.Resources;
using Tooling.Utility;

namespace Tooling.Features.ProjectRenamer.Views
{
	/// <summary>
	/// Interaction logic for ProjectRenameDialogWindow.xaml.
	/// </summary>
	public partial class ProjectRenameDialogWindow
	{
		public ProjectRenameDialogWindow()
		{
			InitializeComponent();
			this.ShouldBeThemed();
		}

		private void Button1_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(string.Format(CultureInfo.CurrentUICulture, "We are inside {0}.Button1_Click()", this.ToString()));
		}
	}


	[Guid("fea518ce-68b4-46a2-ad89-694794a37694")]
	public class ProjectRenameDialogToolPane : ToolWindowPane
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectMover.Views.ProjectMoverToolWindow"/> class.
		/// </summary>
		public ProjectRenameDialogToolPane() : base(null)
		{
			this.Caption = Translations.title_RenameProject;

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
			// the object returned by the Content property.
			this.Content = new ProjectRenameDialogWindow();
		}
	}
}
