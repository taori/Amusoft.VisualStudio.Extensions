using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Extensibility;
using Company.Desktop.Framework.Mvvm.Navigation;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.Mvvm.ViewModels
{
	public abstract class ContentViewModel : InteractiveViewModel, IServiceProviderHolder, IContentViewModel
	{
		protected static readonly ILogger Log = LogManager.GetLogger(nameof(ContentViewModel));

		/// <inheritdoc />
		public bool Activated { get; private set; }

		/// <inheritdoc />
		public async Task ActivateAsync(IActivationContext context)
		{
			await OnActivateAsync(context);

			Activated = true;
			if (OnActivated != null)
				await OnActivated?.Invoke(this, EventArgs.Empty);
		}

		/// <inheritdoc />
		public event AsyncEventHandler OnActivated;

		protected abstract Task OnActivateAsync(IActivationContext context);

		protected async Task UpdateRegionAsync(IContentViewModel content, string regionName)
		{
			using (LoadingState.Session())
			{
				var regionManager = ServiceProvider.GetRequiredService<IRegionManager>();
				var visualizer = ServiceProvider.GetRequiredService<IViewModelVisualizer>();
				var control = regionManager.GetControl(this, regionName);
				await visualizer.VisualizeAsync(content, control);
			}
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; set; }
	}
}