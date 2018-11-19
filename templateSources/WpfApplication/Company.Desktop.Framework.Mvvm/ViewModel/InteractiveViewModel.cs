using System.Collections.Generic;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Interactivity;

namespace Company.Desktop.Framework.Mvvm.ViewModel
{
	public abstract class InteractiveViewModel : ViewModelBase, IInteractive, IBusyStateHolder
	{
		protected InteractiveViewModel()
		{
			Initialize();
		}

		private void Initialize()
		{
			InitializeBehaviours();
		}

		protected abstract void InitializeBehaviours();

		public List<IBehaviour> Behaviours { get; } = new List<IBehaviour>();

		/// <inheritdoc />
		public BusyState LoadingState { get; } = new BusyState();
	}
}