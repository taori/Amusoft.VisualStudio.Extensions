﻿using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;
using JetBrains.Annotations;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public class FrameworkElementCoordinator : IDisplayCoordinator
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(FrameworkElementCoordinator));

		public IRegionManager RegionManager { get; }
		public IViewComposerFactory ComposerFactory { get; }
		public IServiceContext ServiceContext { get; }
		public IBehaviourRunner BehaviourRunner { get; }

		public FrameworkElementCoordinator(IRegionManager regionManager, IViewComposerFactory composerFactory, IServiceContext serviceContext, [NotNull] IBehaviourRunner behaviourRunner)
		{
			RegionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
			ComposerFactory = composerFactory ?? throw new ArgumentNullException(nameof(composerFactory));
			ServiceContext = serviceContext ?? throw new ArgumentNullException(nameof(serviceContext));
			BehaviourRunner = behaviourRunner ?? throw new ArgumentNullException(nameof(behaviourRunner));
		}

		/// <inheritdoc />
		public int Priority => 0;

		/// <inheritdoc />
		public bool CanProcess(object dataContext)
		{
			return dataContext is IContentViewModel;
		}

		/// <inheritdoc />
		public async Task<bool> DisplayAsync(object dataContext, ICoordinationArguments coordinationArguments)
		{
			if (coordinationArguments is RegionArguments arguments)
			{
				var control = RegionManager.GetControl(arguments.RegionManagerReference, arguments.TargetRegion);

				if (control.DataContext is IBehaviourHost interactive)
				{
					var context = new ContentChangingBehaviourContext(ServiceContext.ServiceProvider, control.DataContext, dataContext);
					await BehaviourRunner.ExecuteAsync(interactive, context);
					if (context.Cancelled)
					{
						Log.Error($"Change prevented by {nameof(ContentChangingBehaviourContext)}.");
						return false;
					}
				}

				var composer = ComposerFactory.Create(control);
				if (composer == null)
					return false;

				return await composer.ComposeAsync(new ViewCompositionContext(control, dataContext, coordinationArguments));
			}

			return false;
		}
	}
}