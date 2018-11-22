using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Extensions;
using Company.Desktop.Framework.Mvvm.Interactivity.Window;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public class WindowComposer : ViewComposerBase
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(WindowComposer));

		/// <inheritdoc />
		public WindowComposer(IServiceContext serviceContext, IEnumerable<IViewComposerHook> composerHooks, IBehaviourRunner behaviourRunner) : base(serviceContext, composerHooks, behaviourRunner)
		{
		}

		/// <param name="context"></param>
		/// <inheritdoc />
		protected override Task FinalizeCompositionAsync(IViewCompositionContext context)
		{
			if(context.Control is Window window)
			{
				context.Control.DataContext = context.DataContext;

				if(context.DataContext is IWindowViewModel windowViewModel && windowViewModel.ClaimMainWindowOnOpen)
					Application.Current.MainWindow = window;

				window.Show();
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override void Configure(IViewCompositionContext context)
		{
			if (context.Control is Window window)
			{
				if (context.DataContext is IWindowViewModel windowViewModel)
				{
					windowViewModel.WhenFocusRequested.Subscribe(o => window.Focus());
					windowViewModel.WhenClosingRequested.Subscribe(o => window.Close());
					windowViewModel.WhenMinimizeRequested.Subscribe(o => window.WindowState = WindowState.Minimized);
					windowViewModel.WhenMaximizeRequested.Subscribe(o => window.WindowState = WindowState.Maximized);
					windowViewModel.WhenNormalizeRequested.Subscribe(o => window.WindowState = WindowState.Normal);

					var windowClosed = Observable.FromEventPattern<EventHandler, EventArgs>(d => window.Closed += d, d => window.Closed -= d).Select(s => s.EventArgs);
					var windowClosing = Observable.FromEventPattern<CancelEventHandler, CancelEventArgs>(d => window.Closing += d, d => window.Closing -= d).Select(s => s.EventArgs);
					var windowStateChanged = Observable.FromEventPattern<EventHandler, EventArgs>(d => window.StateChanged += d, d => window.StateChanged -= d).Select(s => window.WindowState);

					windowClosing.Subscribe(args =>
					{
						if (windowViewModel is IWindowListener listener)
							listener.NotifyClosing(args);
					});
					windowStateChanged.Subscribe(args =>
					{
						if (windowViewModel is IWindowListener listener)
							listener.NotifyWindowStateChange(args);;
					});
					windowClosed.Subscribe(async (args) =>
					{
						if (windowViewModel is IWindowListener listener)
							listener.NotifyClosed();

						await BehaviourRunner.ExecuteAsync(windowViewModel as IBehaviourHost, new WindowClosedContext(windowViewModel, ServiceContext.ServiceProvider));
					});
				}
			}
		}

		/// <inheritdoc />
		public override bool CanHandle(FrameworkElement control)
		{
			return control is Window;
		}
	}
}