using System.Windows;
using System.Windows.Media;

namespace Company.Desktop.Framework.Mvvm.Extensions
{
	public static class FrameworkElementExtensions
	{
		public static T GetParentOfType<T>(this FrameworkElement source)
			where T : DependencyObject
		{
			var current = source.Parent;
			
			while(current != null)
			{
				if (current is T casted)
					return casted;

				current = VisualTreeHelper.GetParent(current);
			}

			return default(T);
		}
	}
}