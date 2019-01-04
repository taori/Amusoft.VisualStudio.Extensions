using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Commands;

namespace Company.Desktop.Framework.Mvvm.Commands
{
	public delegate bool CanExecuteHandler<TParameter>(TParameter parameter);
	public delegate Task ExecuteHandler<TParameter>(TParameter parameter);

	public class TaskExecution : TaskExecution<object>
	{
		/// <inheritdoc />
		public TaskExecution(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute) : base(execute, canExecute)
		{
		}

		/// <inheritdoc />
		public TaskExecution(ExecuteHandler<object> execute) : this (execute, p => true)
		{
		}
	}

	public class TaskExecution<TParameter> : IBehavior
	{
		private readonly ExecuteHandler<TParameter> _execute;
		private readonly CanExecuteHandler<TParameter> _canExecute;

		/// <inheritdoc />
		public TaskExecution(ExecuteHandler<TParameter> execute, CanExecuteHandler<TParameter> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		/// <inheritdoc />
		public TaskExecution(ExecuteHandler<TParameter> execute) : this (execute, p => true)
		{
			_execute = execute;
		}

		/// <inheritdoc />
		public bool CanExecute(object parameter)
		{
			if (parameter == null)
				return _canExecute.Invoke(default(TParameter));

			return _canExecute.Invoke((TParameter)parameter);
		}

		/// <inheritdoc />
		public async Task ExecuteAsync(object parameter)
		{
			if (parameter == null)
			{
				await _execute.Invoke(default(TParameter));
			}
			else
			{
				await _execute.Invoke((TParameter)parameter);
			}
		}

		/// <inheritdoc />
		public Task AllExecutedAsync(object parameter)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task AllExecutingAsync(object parameter)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task OnExecutingAsync(object parameter)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task OnExecutedAsync(object parameter)
		{
			return Task.CompletedTask;
		}
	}
}