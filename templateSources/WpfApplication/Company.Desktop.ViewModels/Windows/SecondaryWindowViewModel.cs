

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors;
using Company.Desktop.Framework.Mvvm.ViewModel;

namespace Company.Desktop.ViewModels.Windows
{
	public class SecondaryWindowViewModel : WindowContentViewModelBase, IConfigureWindow
	{
		private byte[] _someMemory;
		
		/// <inheritdoc />
		protected override async Task OnActivateAsync(IActivationContext context)
		{
			_someMemory = new byte[200000000];
			await Task.Delay(3000);
			Window.Title = $"Title updated: {DateTime.Now.ToString("F")} {_someMemory.Length}";
		}

		/// <inheritdoc />
		public override IEnumerable<IBehavior> GetDefaultBehaviors()
		{
			yield return new RestoreWindowDimensionsBehavior();
			yield return new DisposeOnCloseBehavior();
		}

		/// <inheritdoc />
		public override string GetTitle()
		{
			return $"This is a secondary window";
		}

		/// <inheritdoc />
		public void Configure(IWindowViewModel window)
		{
			window.ShowInTaskbar = false;
		}
	}
}