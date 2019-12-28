using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Data;
using Company.Desktop.Framework.Mvvm.Extensions;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	/// <summary>
	/// This behavior can be applied if you want a window to have a size restoration behavior
	/// </summary>
	public class RestoreWindowDimensionsBehavior : AsyncBehaviorBase<IViewComposedBehaviorContext>
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(RestoreWindowDimensionsBehavior));

		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IViewComposedBehaviorContext context)
		{
			if(context.CompositionContext.CoordinationArguments is WindowArguments windowArguments)
			{
				var windowStorageKey = $"{nameof(WindowArguments)}.{windowArguments.WindowId}";
				if (context.ServiceContext.ServiceProvider.GetService<ISettingsStorage>() is ISettingsStorage storage)
				{
					if (storage.TryGetValue(windowStorageKey, out WindowAttributes value))
					{
						Log.Debug($"Restoring window with argument [{windowArguments.WindowId}] at [{value.Left};{value.Top}] with [{value.Width};{value.Height}]");
						
						if (context.CompositionContext.Control is System.Windows.Window updateWindow)
						{
							updateWindow.SetCurrentValue(System.Windows.Window.LeftProperty, value.Left);
							updateWindow.SetCurrentValue(System.Windows.Window.TopProperty, value.Top);
							updateWindow.SetCurrentValue(System.Windows.FrameworkElement.WidthProperty, value.Width);
							updateWindow.SetCurrentValue(System.Windows.FrameworkElement.HeightProperty, value.Height);
						}
					}
				}
				else
				{
					Log.Error($"No {nameof(ISettingsStorage)} available.");
					return;
				}

				if (context.CompositionContext.Control is System.Windows.Window window)
				{
					window.WhenSizeChanged()
						.Select(d => EventArgs.Empty)
						.Merge(window.WhenLocationChanged().Select(d => EventArgs.Empty))
						.Throttle(TimeSpan.FromMilliseconds(250))
						.ObserveOn(Application.Current.Dispatcher)
						.Subscribe(d =>
						{
							Log.Debug($"Updating window size information for [{windowArguments.WindowId}].");
							storage.UpdateValue(windowStorageKey, new WindowAttributes(window.Width, window.Height, window.Left, window.Top));
							storage.Save();
						});
				}
			}
		}
	}
}
