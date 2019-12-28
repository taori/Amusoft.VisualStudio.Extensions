using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public class FrameworkElementComposer : ViewComposerBase
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(FrameworkElementComposer));

		public IEnumerable<IViewContextBinder> ViewContextBinder { get; }

		/// <inheritdoc />
		public FrameworkElementComposer(IServiceContext serviceContext, 
			IEnumerable<IViewComposerHook> composerHooks, 
			IEnumerable<IViewContextBinder> viewContextBinder, 
			IBehaviorRunner behaviorRunner) 
			: base(serviceContext, composerHooks, behaviorRunner)
		{
			ViewContextBinder = viewContextBinder;
		}

		/// <param name="context"></param>
		/// <inheritdoc />
		protected override Task FinalizeCompositionAsync(IViewCompositionContext context)
		{
			if (context.Control is ContentPresenter contentPresenter)
			{
				contentPresenter.Content = context.DataContext;
			}
			else
			{
				context.Control.DataContext = context.DataContext;
			}

			foreach (var binder in ViewContextBinder)
			{
				if (binder.TryBind(context))
				{
					Log.Debug($"ViewBinder {binder.GetType().FullName} is handling {context}.");
				}
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override void Configure(IViewCompositionContext context)
		{
		}

		/// <inheritdoc />
		public override bool CanHandle(FrameworkElement control)
		{
			if (control is FrameworkElement && !(control is Window))
				return true;
			return false;
		}
	}
}