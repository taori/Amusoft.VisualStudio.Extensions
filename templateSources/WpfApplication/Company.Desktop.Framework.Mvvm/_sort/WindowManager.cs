using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.UI;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class WindowManager : IWindowManager
	{
		public static readonly DependencyProperty WindowManagerIdProperty = DependencyProperty.RegisterAttached(
			"WindowManagerId", typeof(string), typeof(WindowManager), new PropertyMetadata(default(string)));

		public static void SetWindowManagerId(DependencyObject element, string value)
		{
			element.SetValue(WindowManagerIdProperty, value);
		}

		public static string GetWindowManagerId(DependencyObject element)
		{
			return (string) element.GetValue(WindowManagerIdProperty);
		}

		/// <inheritdoc />
		public void RegisterWindow(Window window, string id)
		{
			SetWindowManagerId(window, id);
		}

		/// <inheritdoc />
		public bool TryGetWindow(string id, out Window window)
		{
			foreach (Window currentWindow in Application.Current.Windows)
			{
				var windowManagerId = GetWindowManagerId(currentWindow);
				if (string.IsNullOrEmpty(windowManagerId))
					continue;
				if (string.Equals(id, windowManagerId))
				{
					window = currentWindow;
					return true;
				}
			}

			window = null;
			return false;
		}
	}
}