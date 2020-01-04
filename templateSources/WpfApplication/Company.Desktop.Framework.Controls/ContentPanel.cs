using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Company.Desktop.Framework.Controls
{
	[ContentProperty(nameof(ContentControl.Content))]
	public class ContentPanel : ContentControl
	{
		static ContentPanel()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentPanel),
				new FrameworkPropertyMetadata(typeof(ContentPanel)));
		}
	}
}