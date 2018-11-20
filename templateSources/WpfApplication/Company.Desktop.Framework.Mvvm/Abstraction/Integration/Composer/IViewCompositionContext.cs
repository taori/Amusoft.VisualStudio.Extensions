using System.Windows;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer
{
	public interface IViewCompositionContext
	{
		FrameworkElement Control { get; }

		object DataContext { get; }
	}
}