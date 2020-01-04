using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Company.Desktop.Framework.Controls
{
	[TemplatePart(Name = "PART_Left", Type = typeof(StackPanel))]
	[TemplatePart(Name = "PART_Right", Type = typeof(StackPanel))]
	[ContentProperty(nameof(LeftItems))]
	public class ToolbarPanel : Control
	{
		static ToolbarPanel()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolbarPanel), new FrameworkPropertyMetadata(typeof(ToolbarPanel)));
		}

		public static readonly DependencyProperty ItemsSpacingProperty = DependencyProperty.Register(
			nameof(ItemsSpacing), typeof(Thickness), typeof(ToolbarPanel), new PropertyMetadata(default(Thickness)));

		public Thickness ItemsSpacing
		{
			get { return (Thickness)GetValue(ItemsSpacingProperty); }
			set { SetValue(ItemsSpacingProperty, value); }
		}

		private UIElementCollection _leftItems;
		public UIElementCollection LeftItems
		{
			get { return _leftItems ?? (_leftItems = new UIElementCollection(this, this)); }
		}

		private UIElementCollection _rightItems;
		public UIElementCollection RightItems
		{
			get { return _rightItems ?? (_rightItems = new UIElementCollection(this, this)); }
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			MoveItems("PART_Left", LeftItems);
			MoveItems("PART_Right", RightItems);
		}

		private void MoveItems(string partName, UIElementCollection sourceCollection)
		{
			var stackPanel = Template.FindName(partName, this) as StackPanel;
			if (stackPanel == null)
				return;

			var items = new List<UIElement>();
			foreach (UIElement uiElement in sourceCollection)
			{
				items.Add(uiElement);
			}

			foreach (UIElement item in items)
			{
				sourceCollection.Remove(item);
				stackPanel.Children.Add(item);
			}
		}
	}
}