using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
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
						if (await deactivationSession.IsCancelledAsync(dataContext as IInteractive, ServiceContext.ServiceProvider))
							return;

						WindowDeactivatorSession.SetCloseChecksPassed(sender as DependencyObject, true);

						// workaround invalidoperationexception
						await Task.Delay(50);
						(sender as Window)?.Close();
					}
				};
				window.Closing += windowOnClosing;
			}
		}
	}
}