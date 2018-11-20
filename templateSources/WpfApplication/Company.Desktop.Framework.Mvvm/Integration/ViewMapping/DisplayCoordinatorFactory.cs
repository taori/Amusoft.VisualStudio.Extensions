using System;
using System.Collections.Generic;
using System.Linq;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
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

			var coordinator = Coordinators.OrderByDescending(d => d.FactoryOrder).FirstOrDefault(d => d.CanProcess(dataContext));
			if (coordinator == null)
			{
				Log.Error($"No implementation of {typeof(IDisplayCoordinator).FullName} can process {dataContext.GetType().FullName}.");
				return null;
			}

			Log.Error($"{coordinator.GetType().FullName} is used to render {dataContext.GetType().FullName}.");
			return coordinator;
		}
	}
}