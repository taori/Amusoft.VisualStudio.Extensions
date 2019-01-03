using System;
using System.Collections.Generic;
using System.Linq;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public class DisplayCoordinatorFactory : IDisplayCoordinatorFactory
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(DisplayCoordinatorFactory));

		public IEnumerable<IDisplayCoordinator> Coordinators { get; }

		public DisplayCoordinatorFactory(IEnumerable<IDisplayCoordinator> visualizers)
		{
			Coordinators = visualizers ?? throw new ArgumentNullException(nameof(visualizers));
		}

		/// <inheritdoc />
		public IDisplayCoordinator Create(object dataContext)
		{
			if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));

			Log.Debug($"Creating coordinator, sorted by descending {nameof(IDisplayCoordinator.Priority)}.");
			var coordinator = Coordinators.OrderByDescending(d => d.Priority).FirstOrDefault(d => d.CanProcess(dataContext));
			if (coordinator == null)
			{
				Log.Error($"No implementation of {typeof(IDisplayCoordinator).FullName} can process {dataContext.GetType().FullName}. You might need to maintain your implementation of {nameof(IInjectionAssemblyLoader)}.");
				return null;
			}

			Log.Debug($"{coordinator.GetType().FullName} is used to render {dataContext.GetType().FullName}.");
			return coordinator;
		}
	}
}