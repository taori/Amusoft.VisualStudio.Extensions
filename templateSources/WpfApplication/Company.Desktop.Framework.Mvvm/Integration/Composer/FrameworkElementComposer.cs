using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public class FrameworkElementComposer : ViewComposerBase
	{
		/// <inheritdoc />
		public FrameworkElementComposer(IServiceContext serviceContext, IEnumerable<IViewComposerHook> composerHooks, IBehaviorRunner behaviorRunner) : base(serviceContext, composerHooks, behaviorRunner)
		{
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