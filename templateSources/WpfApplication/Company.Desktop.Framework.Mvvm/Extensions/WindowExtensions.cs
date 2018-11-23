using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Extensions
{
	public static class WindowExtensions
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(WindowExtensions));

		public static IObservable<EventArgs> WhenClosed(this Window source)
		{
			Log.Trace($"subscribed {nameof(WhenClosed)}.");
			return Observable
				.FromEventPattern<EventHandler, EventArgs>(d => source.Closed += d, d => source.Closed -= d)
				.Select(s => s.EventArgs);
		}

		public static IObservable<CancelEventArgs> WhenClosing(this Window source)
		{
			Log.Trace($"subscribed {nameof(WhenClosing)}.");
			return Observable
				.FromEventPattern<CancelEventHandler, CancelEventArgs>(d => source.Closing += d, d => source.Closing -= d)
				.Select(s => s.EventArgs);
		}

		public static IObservable<WindowState> WhenStateChanged(this Window source)
		{
			Log.Trace($"subscribed {nameof(WhenStateChanged)}.");
			return Observable
				.FromEventPattern<EventHandler, EventArgs>(d => source.StateChanged += d, d => source.StateChanged -= d)
				.Select(s => source.WindowState);
		}

		public static IObservable<SizeChangedEventArgs> WhenSizeChanged(this Window source)
		{
			Log.Trace($"subscribed {nameof(WhenSizeChanged)}.");
			return Observable
				.FromEventPattern<SizeChangedEventHandler, SizeChangedEventArgs>(d => source.SizeChanged += d, d => source.SizeChanged -= d)
				.Select(s => s.EventArgs);
		}

		public static IObservable<Point> WhenLocationChanged(this Window source)
		{
			Log.Trace($"subscribed {nameof(WhenLocationChanged)}.");
			return Observable
				.FromEventPattern<EventHandler, EventArgs>(d => source.LocationChanged += d, d => source.LocationChanged -= d)
				.Select(s => new Point(source.Left, source.Top));
		}
	}
}