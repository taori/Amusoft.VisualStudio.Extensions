using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using Company.Desktop.Framework.Mvvm.Utility;
using Microsoft.Xaml.Behaviors;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviors
{
	public class KeepWindowInWorkspaceBehavior : Behavior<System.Windows.Window>
	{
		public static readonly DependencyProperty ThrottleDelayProperty = DependencyProperty.Register(
			nameof(ThrottleDelay), 
			typeof(TimeSpan), 
			typeof(KeepWindowInWorkspaceBehavior),
			new PropertyMetadata(TimeSpan.FromMilliseconds(500)));

		public TimeSpan ThrottleDelay
		{
			get { return (TimeSpan) GetValue(ThrottleDelayProperty); }
			set { SetValue(ThrottleDelayProperty, value); }
		}

		private Subject<object> _whenChanged = new Subject<object>();
		public IObservable<object> WhenChanged => _whenChanged;
		/// <inheritdoc />
		protected override void OnAttached()
		{
			WhenChanged
				.Throttle(ThrottleDelay)
				.ObserveOn(Dispatcher)
				.Subscribe(UpdateRequired);
			this.AssociatedObject.Loaded += Loaded;
			this.AssociatedObject.LocationChanged += LocationChanged;
			this.AssociatedObject.SizeChanged += SizeChanged;
			base.OnAttached();
		}

		private void UpdateRequired(object obj)
		{
			var intersects = ScreenHelper.GetXIntersects(AssociatedObject);
			var top = this.AssociatedObject.Top;
			var height = this.AssociatedObject.Height;
			var allRects = ScreenHelper.GetTaskbarRects();
			foreach (var screen in intersects)
			{
				if (allRects.TryGetValue(screen, out var rect))
					if (top + height > rect.Y)
					{
						AssociatedObject.Top = 0;
						AssociatedObject.MaxHeight = Math.Min(rect.Y, AssociatedObject.Height);
						AssociatedObject.MaxHeight = double.PositiveInfinity;
					}
			}
		}

		private void SizeChanged(object sender, SizeChangedEventArgs e)
		{
			_whenChanged.OnNext(null);
		}

		private void LocationChanged(object sender, EventArgs e)
		{
			_whenChanged.OnNext(null);
		}

		private void Loaded(object sender, RoutedEventArgs e)
		{
			_whenChanged.OnNext(null);
		}

		/// <inheritdoc />
		protected override void OnDetaching()
		{
			_whenChanged.OnCompleted();
			_whenChanged.Dispose();
			this.AssociatedObject.Loaded -= Loaded;
			this.AssociatedObject.LocationChanged -= LocationChanged;
			this.AssociatedObject.SizeChanged -= SizeChanged;
			base.OnDetaching();
		}
	}
}