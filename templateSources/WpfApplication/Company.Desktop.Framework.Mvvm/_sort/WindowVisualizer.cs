
using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm._sort;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class WindowVisualizer : IViewModelVisualizer
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(WindowVisualizer));

		public IWindowManager WindowManager { get; }
		public IViewModelActivator Activator { get; }
		public IViewModelWindowFactory WindowFactory { get; }

		public WindowVisualizer(IWindowManager windowManager, IViewModelActivator activator, IViewModelWindowFactory windowFactory)
		{
			WindowManager = windowManager;
			Activator = activator;
			WindowFactory = windowFactory;
		}

		/// <inheritdoc />
		public int FactoryOrder { get; }

		/// <inheritdoc />
		public bool CanProcess(IActivateable activateable)
		{
			return activateable is IWindowViewModel;
		}

		/// <inheritdoc />
		public async Task<bool> VisualizeAsync(IActivateable activateable, ICoordinationArguments coordinationArguments)
		{
			if (activateable == null) throw new ArgumentNullException(nameof(activateable));
			if (coordinationArguments == null) throw new ArgumentNullException(nameof(coordinationArguments));

			if (coordinationArguments is WindowArguments arguments)
			{
				if (!WindowManager.TryGetWindow(arguments.WindowId, out var window))
				{
					Log.Debug($"Creating window for {activateable.GetType().FullName} using id [{arguments.WindowId}]");
					window = WindowFactory.CreateWindow(activateable);
					WindowManager.RegisterWindow(window, arguments.WindowId);
				}

				return await Activator.ActivateAsync(activateable, window);
			}

			Log.Error($"Unable to visualize {activateable} because {nameof(arguments)} is not of type {typeof(WindowArguments).FullName}");
			return false;
		}
	}
}