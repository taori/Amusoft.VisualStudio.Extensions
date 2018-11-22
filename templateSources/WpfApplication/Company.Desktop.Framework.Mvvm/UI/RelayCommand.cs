using System;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace Company.Desktop.Framework.Mvvm.UI
{
	public class RelayCommand : RelayCommand<object>
	{
		/// <inheritdoc />
		public RelayCommand(Action<object> execute) : base(execute)
		{
		}

		/// <inheritdoc />
		public RelayCommand(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute)
		{
		}
	}

	public class RelayCommand<T> : ICommand, IDisposable
	{
		readonly Action<T> _execute = null;
		readonly Predicate<T> _canExecute = null;

		private Subject<T> _whenExecuted = new Subject<T>();
		public IObservable<T> WhenExecuted => _whenExecuted;

		private Subject<T> _whenExecuting = new Subject<T>();
		public IObservable<T> WhenExecuting => _whenExecuting;

		public RelayCommand(Action<T> execute)
			: this(execute, (Predicate<T>)null)
		{
		}

		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute((T)parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter)
		{
			_whenExecuting.OnNext((T)parameter);
			_execute((T)parameter);
			_whenExecuted.OnNext((T)parameter);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_whenExecuted?.Dispose();
			_whenExecuting?.Dispose();
		}
	}
}