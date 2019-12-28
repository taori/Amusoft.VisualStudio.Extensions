using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Interactivity;
using Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using Company.Desktop.Framework.Mvvm.ViewModel;
using JetBrains.Annotations;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public class WindowCoordinator : IDisplayCoordinator
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(WindowCoordinator));

		public IWindowManager WindowManager { get; }
		public IEnumerable<IViewModelWindowFactory> WindowFactories { get; }
		public IViewComposerFactory ComposerFactory { get; }
		public IServiceContext ServiceContext { get; }
		public IBehaviorRunner BehaviorRunner { get; }

		public WindowCoordinator(IWindowManager windowManager, IEnumerable<IViewModelWindowFactory> windowFactories, IViewComposerFactory composerFactory, IServiceContext serviceContext, [NotNull] IBehaviorRunner behaviorRunner)
		{
			WindowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
			WindowFactories = windowFactories ?? throw new ArgumentNullException(nameof(windowFactories));
			ComposerFactory = composerFactory ?? throw new ArgumentNullException(nameof(composerFactory));
			ServiceContext = serviceContext ?? throw new ArgumentNullException(nameof(serviceContext));
			BehaviorRunner = behaviorRunner ?? throw new ArgumentNullException(nameof(behaviorRunner));
		}

		/// <inheritdoc />
		public int Priority => 1;

		/// <inheritdoc />
		public bool CanProcess(object dataContext)
		{
			return dataContext is IWindowViewModel;
		}

		/// <inheritdoc />
		public async Task<bool> DisplayAsync(object dataContext, ICoordinationArguments coordinationArguments)
		{
			if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));
			if (coordinationArguments == null) throw new ArgumentNullException(nameof(coordinationArguments));

			if (coordinationArguments is WindowArguments arguments)
			{
				if (!WindowManager.TryGetWindow(arguments.WindowId, out var window))
				{
					Log.Debug($"Creating window for {dataContext.GetType().FullName} using id [{arguments.WindowId}]");
					foreach (var windowFactory in WindowFactories)
					{
						if (windowFactory.CanCreateWindow(dataContext))
						{
							window = windowFactory.CreateWindow(dataContext);
							WindowManager.RegisterWindow(window, arguments.WindowId);
						}
					}

					if (window == null)
					{
						Log.Error($"No factory was able to create a window for {dataContext.GetType().FullName}");
						return false;
					}
				}

				var composer = ComposerFactory.Create(window);
				if (composer == null)
					return false;

				if (window.DataContext is IBehaviorHost interactive)
				{
					var context = new ContentChangingBehaviorContext(ServiceContext.ServiceProvider, window.DataContext, dataContext);
					await BehaviorRunner.ExecuteAsync(interactive, context);
					if (context.Cancelled)
					{
						Log.Debug($"Change prevented by {nameof(ContentChangingBehaviorContext)}.");
						return false;
					}
				}

				return await composer.ComposeAsync(new ViewCompositionContext(window, dataContext, coordinationArguments));
			}

			Log.Error($"Unable to visualize {dataContext} because {nameof(arguments)} is not of type {typeof(WindowArguments).FullName}");
			return false;
		}
	}
}