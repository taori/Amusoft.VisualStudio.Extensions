using System.Collections.Generic;
using System.Reactive.Disposables;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Interactivity;

namespace Company.Desktop.Framework.Mvvm.ViewModel
{
	public abstract class InteractiveViewModel : ViewModelBase, IBehaviourHost, IBusyStateHolder
	{
		protected readonly CompositeDisposable Disposables = new CompositeDisposable();

		public List<IBehaviour> Behaviours { get; } = new List<IBehaviour>();
		
		/// <inheritdoc />
		public BusyState LoadingState { get; } = new BusyState(); private bool _disposed = false;

		protected virtual void Dispose(bool managedDispose)
		{
			if (!_disposed)
			{
				if (managedDispose)
				{
					foreach (var behaviour in Behaviours)
					{
						behaviour.Dispose();
					}
					Disposables.Dispose();
				}

				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}