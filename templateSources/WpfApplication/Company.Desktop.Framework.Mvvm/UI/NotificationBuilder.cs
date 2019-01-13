using System;
using System.Windows.Input;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;

namespace Company.Desktop.Framework.Mvvm.UI
{
	public class NotificationBuilder
	{
		public Notification Notification { get; }

		internal NotificationBuilder(Notification notification)
		{
			Notification = notification;
		}

		public NotificationBuilder Position(NotificationPosition position)
		{
			Notification.Position = position;
			return this;
		}

		public NotificationBuilder Description(string description)
		{
			Notification.Description = description;
			return this;
		}

		public NotificationBuilder Title(string title)
		{
			Notification.Title = title;
			return this;
		}

		public NotificationBuilder SelectCommand(ICommand command)
		{
			Notification.SelectCommand = command;
			return this;
		}

		public NotificationBuilder CloseCommand(ICommand command)
		{
			Notification.CloseCommand = command;
			return this;
		}

		public NotificationBuilder CloseOnSelect(bool value)
		{
			Notification.CloseOnSelect = value;
			return this;
		}

		public NotificationBuilder AutoClose(TimeSpan delay)
		{
			if(delay < TimeSpan.Zero)
				throw new ArgumentException($"{nameof(delay)} must be greater than zero.");

			Notification.AutoClose = true;
			Notification.AutoCloseDelay = delay;
			return this;
		}
	}
}