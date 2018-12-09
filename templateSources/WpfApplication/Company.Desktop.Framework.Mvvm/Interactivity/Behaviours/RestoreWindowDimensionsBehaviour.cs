using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Data;
using Company.Desktop.Framework.Mvvm.Extensions;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class RestoreWindowDimensionsBehaviour : AsyncBehaviourBase<IViewComposedBehaviourContext>
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(RestoreWindowDimensionsBehaviour));

		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IViewComposedBehaviourContext context)
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
					var changeTrigger = window
						.WhenLocationChanged().Throttle(TimeSpan.FromMilliseconds(250))
						.Select(s => EventArgs.Empty)
						.Concat(window.WhenSizeChanged().Throttle(TimeSpan.FromMilliseconds(250)));
					changeTrigger
						.ObserveOn(System.Windows.Application.Current.Dispatcher)
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