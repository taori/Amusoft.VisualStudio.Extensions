
using System.Threading.Tasks;
using System.Windows;
using Company.Desktop.Framework.Mvvm.ViewModels;

namespace Company.Desktop.Framework.Mvvm.Navigation
{
	public class VisualizerFactory : IViewModelVisualizerFactory
	{
		/// <inheritdoc />
		public IViewModelVisualizer Create(IActivateable activateable)
		{
			throw new System.NotImplementedException();
		}
	}

	public class WindowVisualizer : IViewModelVisualizer
	{
		/// <inheritdoc />
		public async Task<bool> VisualizeAsync(IActivateable activateable, FrameworkElement element)
		{
			throw new System.NotImplementedException();
		}
	}

	public class ContentPresenterVisualizer : IViewModelVisualizer
	{
		/// <inheritdoc />
		public async Task<bool> VisualizeAsync(IActivateable activateable, FrameworkElement element)
		{
			throw new System.NotImplementedException();
		}
	}
}