using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Extensions;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public abstract class ViewComposerBase : IViewComposer
	{
		public IServiceContext ServiceContext { get; }
		public IEnumerable<IViewComposerHook> ComposerHooks { get; }

		protected ViewComposerBase(IServiceContext serviceContext, IEnumerable<IViewComposerHook> composerHooks)
		{
			ServiceContext = serviceContext;
			ComposerHooks = composerHooks;
		}

		/// <inheritdoc />
		public int FactoryPriority { get; }

		protected abstract Task FinalizeCompositionAsync(IViewCompositionContext context);

		private async void ContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is FrameworkElement element)
			{
				element.DataContextChanged -= ContextChanged;
				if (element.DataContext is IServiceProviderHolder holder)
				{
					holder.ServiceProvider = ServiceContext.ServiceProvider;
				}
				
				if (element.DataContext is IBehaviourProvider behaviourProvider && element.DataContext is IInteractive interactiveBehaviour)
				{
					interactiveBehaviour.Behaviours.AddRange(behaviourProvider.GetBehaviours());
				}

				foreach (var hook in ComposerHooks)
				{
					hook.Execute(element, element.DataContext);
				}
				
				if (element.DataContext is IInteractive interactive)
				{
					await interactive.ExecuteBehavioursAsync<IActivationBehaviour, IActivationBehaviourContext>(new ActivationBehaviourContext(interactive, ServiceContext.ServiceProvider));
				}

				if (element.DataContext is ICompositionListener listener)
					listener.Execute(new ViewCompositionContext(element, element.DataContext));
			}
		}

		/// <inheritdoc />
		public async Task<bool> ComposeAsync(IViewCompositionContext context)
		{
			context.Control.DataContextChanged -= ContextChanged;
			context.Control.DataContextChanged += ContextChanged;

			await FinalizeCompositionAsync(context);
			return true;
		}

		/// <inheritdoc />
		public abstract bool CanHandle(FrameworkElement control);
	}
}