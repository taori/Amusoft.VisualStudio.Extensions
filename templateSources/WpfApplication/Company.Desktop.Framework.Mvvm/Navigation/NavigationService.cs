using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.ViewModels;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class NavigationService : INavigationService
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(NavigationService));

		public IViewModelActivator ViewModelActivator { get; }

		public NavigationService(IViewModelActivator viewModelActivator)
		{
			ViewModelActivator = viewModelActivator ?? throw new ArgumentNullException(nameof(viewModelActivator));
		}

		/// <inheritdoc />
		public async Task<bool> OpenWindowAsync(IWindowViewModel viewModel)
		{
			Log.Debug($"Opening window {viewModel.GetType().FullName}");
			return await ViewModelActivator.ActivateAsync(viewModel);
		}
	}
}