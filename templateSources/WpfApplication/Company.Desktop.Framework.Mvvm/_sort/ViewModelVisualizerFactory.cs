using System;
using System.Collections.Generic;
using System.Linq;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm._sort;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class ViewModelVisualizerFactory : IViewModelVisualizerFactory
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ViewModelVisualizerFactory));

		public IEnumerable<IViewModelVisualizer> Visualizers { get; }

		public ViewModelVisualizerFactory(IEnumerable<IViewModelVisualizer> visualizers)
		{
			Visualizers = visualizers ?? throw new ArgumentNullException(nameof(visualizers));
		}

		/// <inheritdoc />
		public IViewModelVisualizer Create(IActivateable activateable)
		{
			if (activateable == null) throw new ArgumentNullException(nameof(activateable));

			foreach (var visualizer in Visualizers.OrderBy(d => d.FactoryOrder))
			{
				if (visualizer.CanProcess(activateable))
					return visualizer;
			}

			Log.Error($"No implementation of {typeof(IViewModelVisualizer).FullName} can process {activateable.GetType().FullName}.");
			return null;
		}
	}
}