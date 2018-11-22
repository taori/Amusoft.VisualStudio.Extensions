using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.Mvvm.ViewModel
{
	public abstract class WindowViewModel : InteractiveViewModel, IServiceProviderHolder, IWindowViewModel, IBehaviourProvider
	{
		protected static readonly ILogger Log = LogManager.GetLogger(nameof(WindowViewModel));

		private string _title;

		public string Title
		{
			get => _title ?? GetWindowTitle();
			set => SetValue(ref _title, value, nameof(Title));
		}

		private double _width = 800;

		public double Width
		{
			get => _width;
			set => SetValue(ref _width, value, nameof(Width));
		}

		private double _height = 450;

		public double Height
		{
			get => _height;
			set => SetValue(ref _height, value, nameof(Height));
		}

		private bool _resizeable = true;

		public bool Resizeable
		{
			get => _resizeable;
			set
			{
				if(SetValue(ref _resizeable, value, nameof(Resizeable)))
					OnPropertyChanged(nameof(ResizeMode));
			}
		}

		private bool _minimizable = true;

		public bool Minimizable
		{
			get => _minimizable;
			set
			{
				if(SetValue(ref _minimizable, value, nameof(Minimizable)))
					OnPropertyChanged(nameof(ResizeMode));
			}
		}

		public ResizeMode ResizeMode
		{
			get
			{
				var mode = ResizeMode.CanMinimize | ResizeMode.CanResize;
				if (!Minimizable)
					mode = mode & ~ResizeMode.CanMinimize;
				if (!Resizeable)
					mode = mode & ~ResizeMode.CanResize;
				return mode;
			}
			set => throw new NotSupportedException();
		}

		private IContentViewModel _content;

		public IContentViewModel Content
		{
			get => _content;
			set => SetValue(ref _content, value, nameof(Content));
		}
		
		public virtual bool ClaimMainWindowOnOpen => false;

		/// <inheritdoc />
		public void Focus()
		{
			Log.Debug($"{nameof(Focus)} requested.");
			_whenFocusRequested?.OnNext(null);
		}

		/// <inheritdoc />
		public void Normalize()
		{
			Log.Debug($"{nameof(Normalize)} requested.");
			_whenNormalizeRequested?.OnNext(null);
		}

		/// <inheritdoc />
		public void Maximize()
		{
			Log.Debug($"{nameof(Maximize)} requested.");
			_whenMaximizeRequested?.OnNext(null);
		}

		/// <inheritdoc />
		public void Minimize()
		{
			Log.Debug($"{nameof(Minimize)} requested.");
			_whenMinimizeRequested?.OnNext(null);
		}

		/// <inheritdoc />
		public void Close()
		{
			Log.Debug($"{nameof(Close)} requested.");
			_whenClosingRequested?.OnNext(null);
		}

		private Subject<object> _whenFocusRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenFocusRequested => _whenFocusRequested;

		private Subject<object> _whenClosingRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenClosingRequested => _whenClosingRequested;

		private Subject<object> _whenNormalizeRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenNormalizeRequested => _whenNormalizeRequested;

		private Subject<object> _whenMinimizeRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenMinimizeRequested => _whenMinimizeRequested;

		private Subject<object> _whenMaximizeRequested = new Subject<object>();
		/// <inheritdoc />
		public IObservable<object> WhenMaximizeRequested => _whenMaximizeRequested;

		private Subject<object> _whenClosed = new Subject<object>();
		public IObservable<object> WhenClosed => _whenClosed;

		private Subject<CancelEventArgs> _whenClosing = new Subject<CancelEventArgs>();
		public IObservable<CancelEventArgs> WhenClosing => _whenClosing;

		private Subject<IActivationContext> _whenActivated = new Subject<IActivationContext>();
		public IObservable<IActivationContext> WhenActivated => _whenActivated;

		private Subject<WindowState> _whenStateChanged = new Subject<WindowState>();
		public IObservable<WindowState> WhenStateChanged => _whenStateChanged;
		
		/// <inheritdoc />
		public async Task ActivateAsync(IActivationContext context)
		{
			await OnActivateAsync(context);
			_whenActivated.OnNext(context);
		}
		
		protected abstract Task OnActivateAsync(IActivationContext context);

		protected abstract string GetWindowTitle();

		protected async Task<bool> UpdateRegionAsync(IContentViewModel content, string regionName)
		{
			Log.Debug($"Updating region [{regionName}] with [{content}]");
			using (LoadingState.Session())
			{
				var visualizerFactory = ServiceProvider.GetRequiredService<IDisplayCoordinatorFactory>();
				var visualizer = visualizerFactory.Create(content);
				return await visualizer.DisplayAsync(content, new RegionArguments(this, regionName));
			}
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; set; }

		/// <inheritdoc />
		public IEnumerable<IBehaviour> GetBehaviours()
		{
			yield break;
		}

		/// <inheritdoc />
		protected override void Dispose(bool managedDispose)
		{
			if (managedDispose)
			{
				_whenFocusRequested.Dispose();
				_whenMaximizeRequested.Dispose();
				_whenMinimizeRequested.Dispose();
				_whenClosingRequested.Dispose();
				_whenNormalizeRequested.Dispose();
				_whenClosed.Dispose();
			}

			base.Dispose(managedDispose);
		}

		/// <inheritdoc />
		public void NotifyClosed()
		{
			Log.Debug(nameof(NotifyClosed));
			_whenClosed.OnNext(null);
		}

		/// <inheritdoc />
		public void NotifyClosing(CancelEventArgs args)
		{
			Log.Debug(nameof(NotifyClosing));
			_whenClosing.OnNext(args);
		}

		/// <inheritdoc />
		public void NotifyWindowStateChange(WindowState args)
		{
			Log.Debug(nameof(NotifyWindowStateChange));
			_whenStateChanged.OnNext(args);
		}
	}
}