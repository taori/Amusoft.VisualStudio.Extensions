using System;
using System.Collections.Generic;
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

		/// <inheritdoc />
		public bool Activated { get; private set; }

		public virtual bool ClaimMainWindowOnOpen => false;

		/// <inheritdoc />
		public void RequestFocus()
		{
			FocusRequested?.Invoke(this, EventArgs.Empty);
		}

		/// <inheritdoc />
		public event EventHandler FocusRequested;

		/// <inheritdoc />
		public async Task ActivateAsync(IActivationContext context)
		{
			await OnActivateAsync(context);
			Activated = !context.Cancelled;
			if (OnActivated != null)
				await OnActivated?.Invoke(this, EventArgs.Empty);
		}

		/// <inheritdoc />
		public event AsyncEventHandler OnActivated;

		protected abstract Task OnActivateAsync(IActivationContext context);

		protected abstract string GetWindowTitle();

		protected async Task UpdateRegionAsync(IContentViewModel content, string regionName)
		{
			using (LoadingState.Session())
			{
				var visualizerFactory = ServiceProvider.GetRequiredService<IDisplayCoordinatorFactory>();
				var visualizer = visualizerFactory.Create(content);
				await visualizer.DisplayAsync(content, new RegionArguments(this, regionName));
			}
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; set; }

		/// <inheritdoc />
		public IEnumerable<IBehaviour> GetBehaviours()
		{
			yield return new RequestClosePermissionBehaviour();
		}
	}
}