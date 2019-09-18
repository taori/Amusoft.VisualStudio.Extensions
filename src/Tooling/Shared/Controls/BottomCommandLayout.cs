using System.ComponentModel;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Tooling.Shared.Controls
{
	[DefaultProperty(nameof(TopArea))]
	public class BottomCommandLayout : ContentControl
	{
		public static readonly DependencyProperty TopAreaProperty = DependencyProperty.Register(
			nameof(TopArea), typeof(object), typeof(BottomCommandLayout), new PropertyMetadata(default(object)));

		public object TopArea
		{
			get { return (object) GetValue(TopAreaProperty); }
			set { SetValue(TopAreaProperty, value); }
		}

		public static readonly DependencyProperty CommandAreaProperty = DependencyProperty.Register(
			nameof(CommandArea), typeof(object), typeof(BottomCommandLayout), new PropertyMetadata(default(object)));

		public object CommandArea
		{
			get { return (object) GetValue(CommandAreaProperty); }
			set { SetValue(CommandAreaProperty, value); }
		}

		public static readonly DependencyProperty CommandLabelAreaProperty = DependencyProperty.Register(
			nameof(CommandLabelArea), typeof(object), typeof(BottomCommandLayout), new PropertyMetadata(default(object)));

		public object CommandLabelArea
		{
			get { return (object) GetValue(CommandLabelAreaProperty); }
			set { SetValue(CommandLabelAreaProperty, value); }
		}
	}
}