using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer.Hooks
{
	public class DataContextLoadedHook : IViewComposerHook
	{
		/// <inheritdoc />
		public void Execute(FrameworkElement control, object dataContext)
		{
			if (dataContext is IInteractive activateable)
			{
				activateable.Behaviours.Add(new ActivationBehaviour());

				if (control is Window window && dataContext is IWindowViewModel windowViewModel)
				{
					if(windowViewModel.ClaimMainWindowOnOpen)
						System.Windows.Application.Current.MainWindow = window;

					window.ResizeMode = windowViewModel.ResizeMode;
				}

			}
		}
	}
}