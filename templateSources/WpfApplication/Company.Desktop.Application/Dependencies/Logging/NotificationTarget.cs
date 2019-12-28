using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Company.Desktop.Application.Dependencies.UI;
using Company.Desktop.Application.Resources;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using Company.Desktop.Framework.Mvvm.UI;
using Company.Desktop.Shared.Extensions;
using NLog;
using NLog.Common;
using NLog.Targets;

namespace Company.Desktop.Application.Dependencies.Logging
{
	[Target("Notification")]
	public class NotificationTarget : TargetWithLayout
	{
		private NotificationService _notificationService;

		private Subject<(string title, string message)> _whenErrorOccured = new Subject<(string title, string message)>();

		private IObservable<(string title, string message)> WhenErrorOccured => _whenErrorOccured;

		public NotificationPosition Position { get; set; } = NotificationPosition.BottomRight;

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			_whenErrorOccured.OnCompleted();
			base.Dispose(disposing);
		}

		private object _lock = new object();
		private bool _bound;
		private void EnsureSubscriberBind()
		{
			if (_bound)
				return;

			lock (_lock)
			{
				if (System.Windows.Application.Current?.Dispatcher == null)
					throw new ArgumentNullException($"Dispatcher is not available yet.");

				WhenErrorOccured
					.Buffer(TimeSpan.FromSeconds(3), 50)
					.ObserveOn(System.Windows.Application.Current.Dispatcher)
					.Subscribe(OnErrorBatch);

				_bound = true;
			}
		}

		/// <inheritdoc />
		protected override void WriteAsyncThreadSafe(AsyncLogEventInfo logEvent)
		{
			try
			{
				EnsureSubscriberBind();
				base.WriteAsyncThreadSafe(logEvent);
			}
			catch (ArgumentNullException)
			{
				throw new Exception(logEvent.LogEvent.ToString());
			}
		}

		/// <inheritdoc />
		protected override void Write(IList<AsyncLogEventInfo> logEvents)
		{
			try
			{
				EnsureSubscriberBind();
				base.Write(logEvents);
			}
			catch (ArgumentNullException)
			{
				throw new Exception(logEvents.Select(d => d.LogEvent.ToString()).JoinOn(", "));
			}
		}

		/// <inheritdoc />
		protected override void WriteAsyncThreadSafe(IList<AsyncLogEventInfo> logEvents)
		{
			try
			{
				EnsureSubscriberBind();
				base.WriteAsyncThreadSafe(logEvents);
			}
			catch (ArgumentNullException)
			{
				throw new Exception(logEvents.Select(d => d.LogEvent.ToString()).JoinOn(", "));
			}
		}

		/// <inheritdoc />
		protected override void Write(AsyncLogEventInfo logEvent)
		{
			try
			{
				EnsureSubscriberBind();
				base.Write(logEvent);
			}
			catch (ArgumentNullException)
			{
				throw new Exception(logEvent.LogEvent.ToString());
			}
		}

		private void OnErrorBatch(IList<(string title, string message)> next)
		{
			if (next.Count == 0)
				return;

			if (next.Count == 1)
			{
				var notificationBuilder = Notification
					.PrimaryScreen(next[0].title, next[0].message)
					.Position(Position)
					.CloseOnSelect(false);

				if (_notificationService == null)
					_notificationService = App.DependencyContainer.ServiceProvider.GetService(typeof(INotificationService)) as NotificationService;
				
				_notificationService?.Display(notificationBuilder.Notification);
			}

			if (next.Count > 1)
			{
				var title = next
					.GroupBy(s => s.title)
					.Select(s => s.Key)
					.JoinOn(", ");

				var message = string.Format(Translations.Shared_ErrorsOccured_0, next.Count);

				var notificationBuilder = Notification
					.PrimaryScreen(title, message)
					.Position(Position)
					.CloseOnSelect(false);

				if (_notificationService == null)
					_notificationService = App.DependencyContainer.ServiceProvider.GetService(typeof(INotificationService)) as NotificationService;

				_notificationService?.Display(notificationBuilder.Notification);
			}
		}

		/// <inheritdoc />
		protected override void Write(LogEventInfo logEvent)
		{
			var message = this.Layout.Render(logEvent);
			_whenErrorOccured.OnNext((logEvent.Level.ToString(), message));
		}
	}
}