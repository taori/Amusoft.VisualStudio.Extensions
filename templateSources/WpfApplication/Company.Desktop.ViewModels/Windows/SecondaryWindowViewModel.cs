

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.ViewModel;

namespace Company.Desktop.ViewModels.Windows
{
	public class SecondaryWindowViewModel : WindowViewModel
	{
		private byte[] _someMemory;

		/// <inheritdoc />
		protected override async Task OnActivateAsync(IActivationContext context)
		{
			_someMemory = new byte[200000000];
			await Task.Delay(3000);
			Title = $"Title updated: {DateTime.Now.ToString("F")} {_someMemory.Length}";
		}

		/// <inheritdoc />
		protected override string GetWindowTitle()
		{
			return $"This is a secondary window";
		}

		/// <inheritdoc />
		public override IEnumerable<IBehaviour> GetDefaultBehaviours()
		{
			yield return new DisposeOnCloseBehaviour();
		}
	}
}