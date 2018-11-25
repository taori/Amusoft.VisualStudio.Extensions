using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer
{
	public interface IViewCompositionContext
	{
		FrameworkElement Control { get; }

		object DataContext { get; }

		ICoordinationArguments CoordinationArguments { get; }
	}
}