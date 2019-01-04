using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Company.Desktop.Framework.Mvvm.Abstraction.Commands;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Commands
{
	public class DisableWhileExecutingBehavior : IBehavior
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(DisableWhileExecutingBehavior));

		private long _executing;

		/// <inheritdoc />
		public bool CanExecute(object parameter)
		{
			var canExecute = Interlocked.Read(ref _executing) == 0;
			return canExecute;
		}

		/// <inheritdoc />
		public Task ExecuteAsync(object parameter)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task AllExecutedAsync(object parameter)
		{
			Interlocked.Decrement(ref _executing);
			CommandManager.InvalidateRequerySuggested();
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task AllExecutingAsync(object parameter)
		{
			Interlocked.Increment(ref _executing);
			CommandManager.InvalidateRequerySuggested();
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