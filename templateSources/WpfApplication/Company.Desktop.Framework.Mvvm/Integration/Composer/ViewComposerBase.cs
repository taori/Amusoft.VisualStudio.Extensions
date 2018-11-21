using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Extensions;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public abstract class ViewComposerBase : IViewComposer
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ViewComposerBase));

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

		private async void DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is FrameworkElement element)
			{
				element.DataContextChanged -= DataContextChanged;
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

				DataContextLoaded(new ViewCompositionContext(element, element.DataContext));
			}
		}

		protected virtual void DataContextLoaded(IViewCompositionContext context) { }

		/// <inheritdoc />
		public async Task<bool> ComposeAsync(IViewCompositionContext context)
		{
			try
			{
				Configure(context);

				context.Control.DataContextChanged -= DataContextChanged;
				context.Control.DataContextChanged += DataContextChanged;

				// memory leak countermeasure
				context.Control.Unloaded += ControlOnUnloaded;

				await FinalizeCompositionAsync(context);
				return true;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return false;
			}
		}

		private void ControlOnUnloaded(object sender, RoutedEventArgs e)
		{
			if (sender is FrameworkElement frameworkElement)
			{
				frameworkElement.Unloaded -= ControlOnUnloaded;
				frameworkElement.DataContextChanged -= DataContextChanged;
			}
		}

		protected abstract void Configure(IViewCompositionContext context);

		/// <inheritdoc />
		public abstract bool CanHandle(FrameworkElement control);
	}
}