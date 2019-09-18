using System;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Tooling.Shared.Helpers
{
	internal static class ToolWindowHelper
	{
		public static void Prompt<TToolWindow>(AsyncPackage package, object dataContext = null, bool createIfNotExists = true)
			where TToolWindow : ToolWindowPane
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			// Get the instance number 0 of this tool window. This window is single instance so this instance
			// is actually the only one.
			// The last flag is set to true so that if the tool window does not exists it will be created.
			ToolWindowPane window = package.FindToolWindow(typeof(TToolWindow), 0, createIfNotExists);
			if ((window == null) || (window.Frame == null))
			{
				throw new NotSupportedException("Cannot create tool window");
			}

			if (window.Content is UserControl control)
				control.DataContext = dataContext;

			IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
			Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
		}
	}
}