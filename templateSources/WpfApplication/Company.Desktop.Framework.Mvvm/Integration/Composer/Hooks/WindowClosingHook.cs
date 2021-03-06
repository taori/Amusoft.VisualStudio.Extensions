﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Interactivity;
using Company.Desktop.Framework.Mvvm.Interactivity.Window;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer.Hooks
{
	public class WindowClosingHook : IViewComposerHook
	{
		public IServiceContext ServiceContext { get; }

		public WindowClosingHook(IServiceContext serviceContext)
		{
			ServiceContext = serviceContext;
		}

		/// <inheritdoc />
		public void Execute(FrameworkElement control, object dataContext)
		{
			if (control is Window window)
			{
				EventHandler windowOnClosed = null;
				CancelEventHandler windowOnClosing = null;
				windowOnClosing = async delegate (object sender, CancelEventArgs args)
				{
					if (WindowDeactivatorSession.GetCloseChecksPassed(sender as DependencyObject))
					{
						window.Closing -= windowOnClosing;
					}
					else
					{
						var deactivationSession = new WindowDeactivatorSession(args);
						if (await deactivationSession.IsCancelledAsync(dataContext as IDeactivate, ServiceContext.ServiceProvider))
							return;
						if (await deactivationSession.IsCancelledAsync(dataContext as IBehaviorHost, ServiceContext.ServiceProvider))
							return;

						WindowDeactivatorSession.SetCloseChecksPassed(sender as DependencyObject, true);

						// workaround invalidoperationexception
						await Task.Delay(50);
						(sender as Window)?.Close();
					}
				};

				windowOnClosed = delegate(object sender, EventArgs args)
				{
					window.Closed -= windowOnClosed;
					window.Closing -= windowOnClosing;
				};

				window.Closed += windowOnClosed;
				window.Closing += windowOnClosing;
			}
		}
	}
}