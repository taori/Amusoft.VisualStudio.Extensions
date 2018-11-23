using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows;

namespace Company.Desktop.Framework.Mvvm.Extensions
{
	public static class WindowExtensions
	{
		public static IObservable<EventArgs> WhenClosed(this Window source)
		{
			return Observable
				.FromEventPattern<EventHandler, EventArgs>(d => source.Closed += d, d => source.Closed -= d)
				.Select(s => s.EventArgs);
		}

		public static IObservable<CancelEventArgs> WhenClosing(this Window source)
		{
			return Observable
				.FromEventPattern<CancelEventHandler, CancelEventArgs>(d => source.Closing += d, d => source.Closing -= d)
				.Select(s => s.EventArgs);
		}

		public static IObservable<WindowState> WhenStateChanged(this Window source)
		{
			return Observable
				.FromEventPattern<EventHandler, EventArgs>(d => source.StateChanged += d, d => source.StateChanged -= d)
				.Select(s => source.WindowState);
		}
	}
}