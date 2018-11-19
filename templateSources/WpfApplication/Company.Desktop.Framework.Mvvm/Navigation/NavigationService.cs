using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Navigation;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm._sort;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class NavigationService : INavigationService
	{
		public IViewModelVisualizerFactory VisualizerFactory { get; }

		private static readonly ILogger Log = LogManager.GetLogger(nameof(NavigationService));

		public NavigationService(IViewModelVisualizerFactory visualizerFactory)
		{
			VisualizerFactory = visualizerFactory;
		}

		/// <inheritdoc />
		public async Task<bool> OpenWindowAsync(IWindowViewModel viewModel, string windowId)
		{
			var visualizer = VisualizerFactory.Create(viewModel);
			Log.Debug($"Opening window {viewModel.GetType().FullName}");
			return await visualizer.VisualizeAsync(viewModel, new WindowArguments(windowId ?? Guid.NewGuid().ToString()));
		}
	}
}