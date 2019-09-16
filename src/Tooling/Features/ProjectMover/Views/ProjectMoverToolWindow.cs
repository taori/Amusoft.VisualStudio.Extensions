using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Tooling.Shared.Resources;

namespace Tooling.Features.ProjectMover.Views
{
	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	/// </summary>
	/// <remarks>
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
	/// usually implemented by the package implementer.
	/// <para>
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its
	/// implementation of the IVsUIElementPane interface.
	/// </para>
	/// </remarks>
	[Guid("2eef1c8b-1f3e-4e6d-b07a-b035986ef0c0")]
	public class ProjectMoverToolWindow : ToolWindowPane
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectMoverToolWindow"/> class.
		/// </summary>
		public ProjectMoverToolWindow() : base(null)
		{
			this.Caption = Translations.ProjectMoverToolWindowTitle;

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
			// the object returned by the Content property.
			this.Content = new ProjectMoverControl();
		}
	}
}
