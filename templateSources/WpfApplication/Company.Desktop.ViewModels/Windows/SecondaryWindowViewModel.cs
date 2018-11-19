

using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.ViewModel;

namespace Company.Desktop.ViewModels.Windows
{
	public class SecondaryWindowViewModel : WindowViewModel
	{
		/// <inheritdoc />
		protected override async Task OnActivateAsync(IActivationContext context)
		{
			await Task.Delay(3000);
			Title = $"Title updated: {DateTime.Now.ToString("F")}";
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