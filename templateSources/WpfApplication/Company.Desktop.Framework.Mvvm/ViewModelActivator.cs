using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Navigation;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.Mvvm
{
	public class ViewModelActivator : IViewModelActivator
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ViewModelActivator));

		public ViewModelActivatorContext Context { get; }

		public ViewModelActivator(ViewModelActivatorContext context)
		{
			Context = context;
		}

		/// <inheritdoc />
		public async Task<bool> ActivateAsync(IActivateable viewModel)
		{
			return await ActivateAsync(viewModel, null);
		}

		/// <inheritdoc />
		public async Task<bool> ActivateAsync(IActivateable viewModel, object view)
		{
			try
			{
				foreach (var activatorFactory in Context.ServiceProvider.GetServices<IViewModelInitializerFactory>())
				{
					if (!activatorFactory.CanHandle(viewModel))
						continue;

					Log.Debug($"{viewModel.GetType().FullName} is requesting activator factory: {activatorFactory.GetType().FullName}");

					var activator = view == null
						? activatorFactory.Create(viewModel)
						: activatorFactory.Create(viewModel, view);

					if (activator == null)
					{
						Log.Error($"No activator could be created for {viewModel.GetType().FullName}");
						return false;
					}

					return await activator.ActivateAsync();
				}

				Log.Error($"No {nameof(IViewModelInitializerFactory)} is available which handles {viewModel.GetType().FullName}");
				return false;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return false;
			}
		}
	}
}