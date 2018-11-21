using System;
using System.Reactive.Subjects;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public abstract class BehaviourBase : IBehaviour
	{
		/// <inheritdoc />
		public int Priority { get; }

		private Subject<object> _executed = new Subject<object>();
		public IObservable<object> Executed => _executed;

		protected void RaiseExecuted()
		{
			if (!_executed.IsDisposed)
				_executed.OnNext(null);
		}

		public void Dispose()
		{
			_executed.Dispose();
		}
	}
}