using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm.ViewModel;
using NLog;
using ActivationContext = Company.Desktop.Framework.Mvvm.Interactivity.ActivationContext;
using Expression = System.Linq.Expressions.Expression;

namespace Company.Desktop.Framework.Mvvm._sort
{
	public abstract class ViewModelInitializerBase<TViewModel, TControl> : IViewModelInitializer
		where TViewModel : IActivateable
		where TControl : FrameworkElement
	{
		public ViewModelInitializerContext Context { get; }
		public TControl Control { get; }
		public TViewModel ViewModel { get; }

		private static readonly ILogger Log = LogManager.GetLogger(nameof(ViewModelInitializerBase<TViewModel, TControl>));

		protected readonly TaskCompletionSource<bool> CompletionSource = new TaskCompletionSource<bool>();

		/// <inheritdoc />
		protected ViewModelInitializerBase(ViewModelInitializerContext context, TControl control, TViewModel viewModel)
		{
			Context = context;
			Control = control;
			ViewModel = viewModel;
		}

		private bool _bound;

		protected virtual void Bind()
		{
			if (_bound)
				return;

			OnBeforeBind();
			OnBind();
			OnAfterBind();

			_bound = true;
		}

		protected virtual void OnBeforeBind() => Expression.Empty();

		protected virtual void OnAfterBind() => Expression.Empty();

		protected virtual void OnBind()
		{
			AttachLoadBehaviour();
			AttachUnloadBehaviour();
		}

		protected abstract void AttachUnloadBehaviour();

		protected virtual void AttachLoadBehaviour()
		{
			DependencyPropertyChangedEventHandler windowOnLoaded = null;
			windowOnLoaded = async delegate (object sender, DependencyPropertyChangedEventArgs args)
			{
				Control.DataContextChanged -= windowOnLoaded;
				AssignViewModelServiceProvider();
				AttachBehavioursInternal();
				InitializeControl();
				await ExecuteActivateAsync();
			};
			Control.DataContextChanged += windowOnLoaded;
		}

		private async Task ExecuteActivateAsync()
		{
			var context = new ActivationContext(Context.ServiceProvider);
			var loadingState = (ViewModel as IBusyStateHolder)?.LoadingState;
			try
			{
				loadingState?.Increment();
				await ViewModel.ActivateAsync(context);
				CompletionSource.TrySetResult(!context.Cancelled);
			}
			catch (Exception e)
			{
				Log.Error(e);
				CompletionSource.TrySetResult(false);
			}
			finally
			{
				loadingState?.Decrement();
			}
		}

		private void AssignViewModelServiceProvider()
		{
			if (ViewModel is IServiceProviderHolder serviceHolder)
				serviceHolder.ServiceProvider = Context.ServiceProvider;
		}

		private void AttachBehavioursInternal()
		{
			var interactive = ViewModel as InteractiveViewModel;
			if (interactive != null)
			{
				AttachBehaviours(interactive.Behaviours);
			}
		}

		protected abstract void AttachBehaviours(List<IBehaviour> behaviours);

		protected abstract void InitializeControl();

		/// <inheritdoc />
		public async Task<bool> ActivateAsync()
		{
			try
			{
				Bind();

				Control.DataContext = ViewModel;
				return await OnActivateAsync() && await CompletionSource.Task;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return false;
			}
		}

		protected abstract Task<bool> OnActivateAsync();
	}
}