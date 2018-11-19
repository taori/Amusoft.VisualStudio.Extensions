using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Extensibility;
using Company.Desktop.Framework.Mvvm.Behaviours;
using Company.Desktop.Framework.Mvvm.ViewModels;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class WindowInitializer<TViewModel> : ViewModelInitializerBase<TViewModel, Window>
		where TViewModel : WindowViewModel
	{
		/// <inheritdoc />
		public WindowInitializer(ViewModelInitializerContext context, Window control, TViewModel viewModel) : base(context, control, viewModel)
		{
			
		}

		/// <inheritdoc />
		protected override void AttachUnloadBehaviour()
		{
			CancelEventHandler windowOnClosing = null;
			windowOnClosing = async delegate (object sender, CancelEventArgs args)
			{
				if (WindowDeactivatorSession.GetCloseChecksPassed(sender as DependencyObject))
				{
					Control.Closing -= windowOnClosing;
				}
				else
				{
					var deactivationSession = new WindowDeactivatorSession(args);
					if (await deactivationSession.IsCancelledAsync(ViewModel as IDeactivate, Context.ServiceProvider))
						return;
					if (await deactivationSession.IsCancelledAsync(ViewModel as IInteractive, Context.ServiceProvider))
						return;

					WindowDeactivatorSession.SetCloseChecksPassed(sender as DependencyObject, true);

					// workaround invalidoperationexception
					await Task.Delay(50);
					(sender as Window)?.Close();
				}
			};
			Control.Closing += windowOnClosing;
		}

		/// <inheritdoc />
		protected override void AttachBehaviours(List<IBehaviour> behaviours)
		{
			behaviours.Add(new CloseTracingBehaviour("Attempting to close window."));
			behaviours.Add(new RequestClosePermissionBehaviour());
		}

		/// <inheritdoc />
		protected override void InitializeControl()
		{
			if (ViewModel.ClaimMainWindowOnOpen)
				System.Windows.Application.Current.MainWindow = Control;

			Control.ResizeMode = ViewModel.ResizeMode;
		}

		/// <inheritdoc />
		protected override Task<bool> OnActivateAsync()
		{
			Control.Show();
			return Task.FromResult(true);
		}
	}
}