using System;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;

namespace Company.Desktop.Framework.Mvvm.Commands
{
	public class TaskCommand : TaskCommand<object>
	{
		/// <inheritdoc />
		public TaskCommand([NotNull] Func<object, Task> execute, [NotNull] Predicate<object> canExecute) : base(execute, canExecute)
		{
		}

		/// <inheritdoc />
		public TaskCommand(Func<object, Task> execute) : base(execute)
		{
		}
	}

	public class TaskCommand<TParameter> : ICommand
	{
		private readonly Func<TParameter, Task> _execute;
		private readonly Predicate<TParameter> _canExecute;

		/// <inheritdoc />
		public TaskCommand([NotNull] Func<TParameter, Task> execute, [NotNull] Predicate<TParameter> canExecute)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
		}

		/// <inheritdoc />
		public TaskCommand(Func<TParameter, Task> execute)
			: this(execute, p => true)
		{
			_execute = execute;
		}

		/// <inheritdoc />
		bool ICommand.CanExecute(object parameter)
		{
			if (parameter == null)
				return _canExecute.Invoke(default(TParameter));
			return _canExecute.Invoke((TParameter)parameter);
		}

		/// <inheritdoc />
		async void ICommand.Execute(object parameter)
		{
			if (parameter == null)
			{
				await _execute(default(TParameter));
			}
			else
			{
				await _execute((TParameter)parameter);
			}
		}

		/// <inheritdoc />
		event EventHandler ICommand.CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}

}