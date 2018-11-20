using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.Mvvm.ViewModel
{
	public abstract class ContentViewModel : InteractiveViewModel, IServiceProviderHolder, IContentViewModel, IBehaviourProvider
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
				var visualizerFactory = ServiceProvider.GetRequiredService<IDisplayCoordinatorFactory>();
				var visualizer = visualizerFactory.Create(content);
				await visualizer.DisplayAsync(content, new RegionArguments(this, regionName));
			}
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; set; }

		/// <inheritdoc />
		public IEnumerable<IBehaviour> GetBehaviours()
		{
			yield return new ConfirmContentChangingBehaviour();
		}
	}
}