using System.Collections.Generic;
using Company.Desktop.Framework.Extensibility;
using Company.Desktop.Framework.Mvvm.UI;

namespace Company.Desktop.Framework.Mvvm.ViewModels
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