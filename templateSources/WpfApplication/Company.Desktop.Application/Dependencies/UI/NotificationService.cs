using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Amusoft.UI.WPF.Notifications;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;
using INotification = Company.Desktop.Framework.Mvvm.Abstraction.UI.INotification;

namespace Company.Desktop.Application.Dependencies.UI
{
	public class NotificationService : INotificationService
	{
		public void Display(INotification notification)
		{
			if (notification is Amusoft.UI.WPF.Notifications.INotification uiNotification)
			{
				var manager = GetManager(notification);
				manager.DisplayAsync(uiNotification, TranslatePosition(notification.Position));
			}
			else
			{
				throw new Exception($"Notification type is not supported because it does not implement Amusoft.UI.WPF.Notifications.INotification.");
			}
		}

		private AnchorPosition TranslatePosition(NotificationPosition position)
		{
			switch (position)
			{
				case NotificationPosition.BottomRight:
					return AnchorPosition.BottomRight;
				case NotificationPosition.Bottom:
					return AnchorPosition.Bottom;
				case NotificationPosition.BottomLeft:
					return AnchorPosition.BottomLeft;
				case NotificationPosition.Left:
					return AnchorPosition.Left;
				case NotificationPosition.TopLeft:
					return AnchorPosition.TopLeft;
				case NotificationPosition.Top:
					return AnchorPosition.Top;
				case NotificationPosition.TopRight:
					return AnchorPosition.TopRight;
				case NotificationPosition.Right:
					return AnchorPosition.Right;
				default:
					throw new ArgumentOutOfRangeException(nameof(position), position, null);
			}
		}

		private NotificationHost GetManager(INotification uiNotification)
		{
			switch (uiNotification.Target)
			{
				case NotificationTarget.PrimaryScreen:
					return NotificationHostManager.GetHostByScreen(Screen.PrimaryScreen);
				case NotificationTarget.CurrentFocusedWindow:
					return NotificationHostManager.GetHostByVisual(GetCurrentFocusedWindow());
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private Visual GetCurrentFocusedWindow()
		{
			foreach (Window window in System.Windows.Application.Current.Windows)
			{
				if (window.IsActive)
					return window;
			}

			return System.Windows.Application.Current.MainWindow;
		}
	}
}