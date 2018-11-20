using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Company.Desktop.Framework.Controls
{
	[ContentProperty(nameof(Content))]
	public class OverlayPanel : ContentControl
	{
		static OverlayPanel()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayPanel), new FrameworkPropertyMetadata(typeof(OverlayPanel)));
		}

		public static readonly DependencyProperty OverlayProperty = DependencyProperty.Register(
			nameof(Overlay), typeof(object), typeof(OverlayPanel), new PropertyMetadata(default(object)));

		public object Overlay
		{
			get { return (object) GetValue(OverlayProperty); }
			set { SetValue(OverlayProperty, value); }
		}

		public static readonly DependencyProperty IsOverlayVisibleProperty = DependencyProperty.Register(
			nameof(IsOverlayVisible), typeof(bool), typeof(OverlayPanel), new FrameworkPropertyMetadata(default(bool)));

		public bool IsOverlayVisible
		{
			get { return (bool) GetValue(IsOverlayVisibleProperty); }
			set { SetValue(IsOverlayVisibleProperty, value); }
		}

		public static readonly DependencyProperty OverlayBackgroundProperty = DependencyProperty.Register(
			nameof(OverlayBackground), typeof(Brush), typeof(OverlayPanel), new FrameworkPropertyMetadata(default(Brush)));

		public Brush OverlayBackground
		{
			get { return (Brush) GetValue(OverlayBackgroundProperty); }
			set { SetValue(OverlayBackgroundProperty, value); }
		}
	}
}
