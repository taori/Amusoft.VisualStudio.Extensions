using System.Windows;
using Company.Desktop.Framework.Mvvm.Interactivity;
using Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using Company.Desktop.Framework.Mvvm.ViewModel;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer.Hooks
{
	public class DataContextLoadedHook : IViewComposerHook
	{
		/// <inheritdoc />
		public void Execute(FrameworkElement control, object dataContext)
		{
			if (dataContext is IBehaviorHost activateable)
			{
				activateable.Behaviors.Add(new ActivationBehavior());

				if (control is Window window && dataContext is IWindowViewModel windowViewModel)
				{
					if(windowViewModel.Content.ClaimMainWindowOnOpen)
						Application.Current.MainWindow = window;

					window.ResizeMode = windowViewModel.ResizeMode;
				}
			}

			if(dataContext is IWindowViewModel subViewModel)
				Execute(control, subViewModel.Content);
		}
	}
}