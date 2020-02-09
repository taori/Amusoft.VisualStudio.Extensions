using System;
using Company.Desktop.Framework.Mvvm.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using JetBrains.Annotations;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class ViewComposedBehaviorContext : IViewComposedBehaviorContext
	{
		public IViewCompositionContext CompositionContext { get; }
		public IServiceProvider ServiceProvider { get; }

		public ViewComposedBehaviorContext([NotNull] IViewCompositionContext compositionContext, [NotNull] IServiceProvider serviceContext)
		{
			CompositionContext = compositionContext ?? throw new ArgumentNullException(nameof(compositionContext));
			ServiceProvider = serviceContext ?? throw new ArgumentNullException(nameof(serviceContext));
		}
	}
}