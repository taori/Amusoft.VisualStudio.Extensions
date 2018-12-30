using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviors
{

	public class FocusOnLoad : Behavior<FrameworkElement>
	{
		public static readonly DependencyProperty FocusTargetProperty = DependencyProperty.Register(
			nameof(FocusTarget), typeof(UIElement), typeof(FocusOnLoad), new PropertyMetadata(default(UIElement)));

		public UIElement FocusTarget
		{
			get { return (UIElement)GetValue(FocusTargetProperty); }
			set { SetValue(FocusTargetProperty, value); }
		}

		/// <inheritdoc />
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
		}

		private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
		{
			FocusTarget.Focus();
			AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
		}
	}
}