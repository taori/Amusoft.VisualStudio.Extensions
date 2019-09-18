using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tooling.Shared.Converters
{
	public class CustomStringToVisibilityConverter : IValueConverter
	{
		public Visibility NullVisibility { get; set; } = Visibility.Collapsed;
		public Visibility NotNullVisibility { get; set; } = Visibility.Visible;

		/// <inheritdoc />
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.IsNullOrEmpty(value?.ToString()) ? NullVisibility : NotNullVisibility;
		}

		/// <inheritdoc />
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}