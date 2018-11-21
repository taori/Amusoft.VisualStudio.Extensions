using System;
using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class DisposeOnCloseBehaviour : ClosedBehaviourBase
	{
		/// <inheritdoc />
		protected override async Task OnExecuteAsync()
		{
			if(Context.ViewModel is IDisposable disposable)
				disposable.Dispose();
		}
	}
}