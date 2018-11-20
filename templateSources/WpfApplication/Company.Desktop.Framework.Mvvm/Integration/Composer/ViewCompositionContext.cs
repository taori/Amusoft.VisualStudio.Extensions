using System.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer;

namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public class ViewCompositionContext : IViewCompositionContext
	{
		/// <inheritdoc />
		public ViewCompositionContext(FrameworkElement control, object dataContext)
		{
			Control = control;
			DataContext = dataContext;
		}

		/// <inheritdoc />
		public FrameworkElement Control { get; }

		/// <inheritdoc />
		public object DataContext { get; }
	}
}