﻿using System;
using System.Reactive.Subjects;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public abstract class BehaviourBase<TArgument> : IBehaviour<TArgument> where TArgument : IBehaviourArgument
	{
		protected abstract void OnExecuteAsync(TArgument context);

		/// <inheritdoc />
		public void Execute(TArgument context)
		{
			OnExecuteAsync(context);
			if (!_whenExecuted.IsDisposed)
				_whenExecuted.OnNext(context);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			Dispose(true);
		}
		
		private bool _disposed;
		protected virtual void Dispose(bool disposeManaged)
		{
			if (_disposed)
				return;
			_whenExecuted.Dispose();
			_disposed = true;
		}

		/// <inheritdoc />
		public int Priority { get; }

		private Subject<object> _whenExecuted = new Subject<object>();
		public IObservable<object> WhenExecuted => _whenExecuted;
	}
}