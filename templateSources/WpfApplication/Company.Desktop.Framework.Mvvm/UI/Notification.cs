using System;
using System.ComponentModel;
using System.Windows.Input;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;

namespace Company.Desktop.Framework.Mvvm.UI
{
	public class Notification : INotification, Amusoft.UI.WPF.Notifications.INotification
	{
		public string Title { get; internal set; }

		public string Description { get; internal set; }

		/// <inheritdoc />
		public NotificationPosition Position { get; internal set; }

		/// <inheritdoc />
		public NotificationTarget Target { get; internal set; }

		/// <inheritdoc />
		public event PropertyChangedEventHandler PropertyChanged;

		/// <inheritdoc />
		public void NotifyDisplayed()
		{
			Displayed?.Invoke(this, EventArgs.Empty);
		}

		/// <inheritdoc />
		public void RequestClose()
		{
			CloseRequested?.Invoke(this, EventArgs.Empty);
		}

		/// <inheritdoc />
		public ICommand CloseCommand { get; internal set; }

		/// <inheritdoc />
		public ICommand SelectCommand { get; internal set; }

		/// <inheritdoc />
		public bool AutoClose { get; internal set; }

		/// <inheritdoc />
		public bool Closed { get; set; }

		/// <inheritdoc />
		public bool CloseOnSelect { get; internal set; }

		/// <inheritdoc />
		public TimeSpan AutoCloseDelay { get; internal set; }

		/// <inheritdoc />
		public event EventHandler CloseRequested;

		/// <inheritdoc />
		public event EventHandler Displayed;

		private static Notification DefaultNotification(NotificationTarget target, string message, string title)
		{
			return new Notification()
			{
				Target = target,
				Description = message,
				Title = title,
				CloseOnSelect = true
			};
		}

		public static NotificationBuilder Window(string title = null, string message = null)
		{
			var notification = DefaultNotification(NotificationTarget.CurrentFocusedWindow, message, title);

			return new NotificationBuilder(notification);
		}

		public static NotificationBuilder PrimaryScreen(string title = null, string message = null)
		{
			var notification = DefaultNotification(NotificationTarget.PrimaryScreen, message, title);

			return new NotificationBuilder(notification);
		}
	}
}