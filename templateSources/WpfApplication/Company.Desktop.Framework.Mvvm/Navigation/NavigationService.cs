using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.ViewModel;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class NavigationService : INavigationService
	{
		public IDisplayCoordinatorFactory CoordinatorFactory { get; }

		private static readonly ILogger Log = LogManager.GetLogger(nameof(NavigationService));

		public NavigationService(IDisplayCoordinatorFactory coordinatorFactory)
		{
			CoordinatorFactory = coordinatorFactory;
		}

		/// <inheritdoc />
		public async Task<bool> OpenWindowAsync(IWindowViewModel viewModel, string windowId)
		{
			var coordinator = CoordinatorFactory.Create(viewModel);
			Log.Debug($"Opening window {viewModel.GetType().FullName}");
			return await coordinator.DisplayAsync(viewModel, new WindowArguments(windowId ?? Guid.NewGuid().ToString()));
		}
	}
}