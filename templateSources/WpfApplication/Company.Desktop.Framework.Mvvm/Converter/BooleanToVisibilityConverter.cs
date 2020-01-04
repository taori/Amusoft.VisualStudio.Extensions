using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Company.Desktop.Framework.Mvvm.Converter
{
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public Visibility TrueVisibility { get; set; } = Visibility.Visible;

		public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool b)
			{
				return b ? TrueVisibility : FalseVisibility;
			}

			throw new NotSupportedException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility visibility)
			{
				if (visibility == TrueVisibility)
					return true;
				return false;
			}

			throw new NotSupportedException();
		}
	}
}