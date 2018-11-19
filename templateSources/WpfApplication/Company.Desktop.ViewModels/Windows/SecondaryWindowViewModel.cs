

using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Navigation;
using Company.Desktop.Framework.Mvvm.ViewModels;
using Company.Desktop.ViewModels.Common;

namespace Company.Desktop.ViewModels.Windows
{
	public class SecondaryWindowViewModel : WindowViewModel
	{
		/// <inheritdoc />
		protected override async Task OnActivateAsync(IActivationContext context)
		{
			
		}

		/// <inheritdoc />
		protected override string GetWindowTitle()
		{
			return $"This is a secondary window";
		}

		/// <inheritdoc />
		protected override void InitializeBehaviours()
		{
		}
	}
}