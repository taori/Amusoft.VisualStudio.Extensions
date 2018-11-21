using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Extensions;
using Company.Desktop.Framework.Mvvm.Interactivity.Window;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public class WindowComposer : ViewComposerBase
	{
		/// <inheritdoc />
		public WindowComposer(IServiceContext serviceContext, IEnumerable<IViewComposerHook> composerHooks) : base(serviceContext, composerHooks)
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
					EventHandler windowOnClosed = null;
					
					windowViewModel.FocusRequested.Subscribe(o => window.Focus());
					windowViewModel.CloseRequested.Subscribe(o => window.Close());
					windowViewModel.MinimizeRequested.Subscribe(o => window.WindowState = WindowState.Minimized);
					windowViewModel.MaximizeRequested.Subscribe(o => window.WindowState = WindowState.Maximized);
					windowViewModel.NormalizeRequested.Subscribe(o => window.WindowState = WindowState.Normal);
					
					windowOnClosed = async delegate(object sender, EventArgs args)
					{
						window.Closed -= windowOnClosed;
						if (windowViewModel is IInteractive interactive)
						{
							var closeContext = new WindowClosedContext(windowViewModel, ServiceContext.ServiceProvider);
							await interactive.ExecuteBehavioursAsync<IWindowClosedBehaviour, IWindowClosedBehaviourContext>(closeContext);
						}
					};

					window.Closed -= windowOnClosed;
					window.Closed += windowOnClosed;
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