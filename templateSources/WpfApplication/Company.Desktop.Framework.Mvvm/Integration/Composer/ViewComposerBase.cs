using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public abstract class ViewComposerBase : IViewComposer
	{
		public static readonly DependencyProperty CoordinationArgumentsProperty = DependencyProperty.RegisterAttached(
			"CoordinationArguments", typeof(ICoordinationArguments), typeof(ViewComposerBase), new PropertyMetadata(default(ICoordinationArguments)));

		public static void SetCoordinationArguments(DependencyObject element, ICoordinationArguments value)
		{
			element.SetValue(CoordinationArgumentsProperty, value);
		}

		public static ICoordinationArguments GetCoordinationArguments(DependencyObject element)
		{
			return (ICoordinationArguments) element.GetValue(CoordinationArgumentsProperty);
		}

		private static readonly ILogger Log = LogManager.GetLogger(nameof(ViewComposerBase));

		public IServiceContext ServiceContext { get; }
		public IEnumerable<IViewComposerHook> ComposerHooks { get; }
		public IBehaviourRunner BehaviourRunner { get; }

		protected ViewComposerBase(IServiceContext serviceContext, IEnumerable<IViewComposerHook> composerHooks, IBehaviourRunner behaviourRunner)
		{
			ServiceContext = serviceContext;
			ComposerHooks = composerHooks;
			BehaviourRunner = behaviourRunner;
		}

		/// <inheritdoc />
		public int Priority { get; }

		protected abstract Task FinalizeCompositionAsync(IViewCompositionContext context);

		private async void DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is FrameworkElement element)
			{
				Log.Debug($"DataContext has changed.");
				element.DataContextChanged -= DataContextChanged;
				if (element.DataContext is IServiceProviderHolder holder)
				{
					Log.Debug($"ServiceProvider set through {nameof(IServiceProviderHolder)}.");
					holder.ServiceProvider = ServiceContext.ServiceProvider;
				}

				if (element.DataContext is IWindowViewModel windowViewModel)
				{
					if (windowViewModel.Content is IServiceProviderHolder subHolder)
						subHolder.ServiceProvider = ServiceContext.ServiceProvider;
				}
				
				if (element.DataContext is IDefaultBehaviourProvider behaviourProvider && element.DataContext is IBehaviourHost interactiveBehaviour)
				{
					var behaviours = behaviourProvider.GetDefaultBehaviours().ToArray();
					if (behaviours.Length > 0)
						Log.Debug($"Binding {behaviours.Length} behaviours through {nameof(IDefaultBehaviourProvider)} [{string.Join(".", behaviours.Select(s => s.GetType().ToString()))}].");
					
					interactiveBehaviour.Behaviours.AddRange(behaviours);
				}

				foreach (var hook in ComposerHooks)
				{
					Log.Debug($"Executing [{nameof(IViewComposerHook)}] -> [{hook.GetType().ToString()}]");
					hook.Execute(element, element.DataContext);
				}

				await BehaviourRunner.ExecuteAsync(element.DataContext as IBehaviourHost, new ActivationBehaviourContext(element.DataContext, ServiceContext.ServiceProvider));
				
				var coordinationArguments = GetCoordinationArguments(element);

				if (element.DataContext is ICompositionListener listener)
					listener.Execute(new ViewCompositionContext(element, element.DataContext, coordinationArguments));

				DataContextLoaded(new ViewCompositionContext(element, element.DataContext, coordinationArguments));
			}
		}

		protected virtual void DataContextLoaded(IViewCompositionContext context) { }

		/// <inheritdoc />
		public async Task<bool> ComposeAsync(IViewCompositionContext context)
		{
			try
			{
				SetCoordinationArguments(context.Control, context.CoordinationArguments);

				Log.Debug($"Composition is being configured.");
				Configure(context);

				Log.Debug($"Attaching DataContextChanged and Unload events.");
				context.Control.DataContextChanged -= DataContextChanged;
				context.Control.DataContextChanged += DataContextChanged;

				// memory leak countermeasure
				context.Control.Unloaded += ControlOnUnloaded;

				Log.Debug($"Finalizing composition.");
				await FinalizeCompositionAsync(context);
				await BehaviourRunner.ExecuteAsync(context.DataContext as IBehaviourHost, new ViewComposedBehaviourContext(context, ServiceContext));

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