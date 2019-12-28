using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public abstract class AsyncBehaviorBase<TContext> : IAsyncBehavior<TContext> where TContext : IBehaviorArgument
	{
		/// <inheritdoc />
		public async Task ExecuteAsync(TContext context)
		{
			await OnExecuteAsync(context);
			if (!_whenExecuted.IsDisposed)
				_whenExecuted.OnNext(context);
		}

		protected abstract Task OnExecuteAsync(TContext context);

		/// <inheritdoc />
		public void Dispose()
		{
			Dispose(true);
		}

		private bool _disposed;
		protected virtual void Dispose(bool disposeManaged)
		{
			if(_disposed)
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