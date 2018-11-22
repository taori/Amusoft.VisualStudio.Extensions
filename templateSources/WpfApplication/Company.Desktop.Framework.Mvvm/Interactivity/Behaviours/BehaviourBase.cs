using System;
using System.Reactive.Subjects;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public abstract class BehaviourBase : IBehaviour
	{
		/// <inheritdoc />
		public int Priority { get; }

		private Subject<object> _whenExecuted = new Subject<object>();
		public IObservable<object> WhenExecuted => _whenExecuted;

		protected void RaiseExecuted()
		{
			if (!_whenExecuted.IsDisposed)
				_whenExecuted.OnNext(null);
		}

		public void Dispose()
		{
			_whenExecuted.Dispose();
		}
	}
}