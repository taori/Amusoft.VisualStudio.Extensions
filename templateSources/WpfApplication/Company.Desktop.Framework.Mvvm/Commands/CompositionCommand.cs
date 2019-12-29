using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Company.Desktop.Framework.Mvvm.Commands
{
	/// <summary>
	/// Command which contains a multitude of command behaviors, to allow for easy disable and callback mechanisms
	/// </summary>
	public class CompositionCommand : ICommand
	{
		/// <summary>
		/// Command which contains a multitude of command behaviors, to allow for easy disable and callback mechanisms
		/// </summary>
		public CompositionCommand()
		{
		}

		/// <summary>
		/// Command which contains a multitude of command behaviors, to allow for easy disable and callback mechanisms
		/// </summary>
		public CompositionCommand(params IAsyncCompositeCommand[] behaviors)
		{
			Compositions.AddRange(behaviors);
		}

		public List<IAsyncCompositeCommand> Compositions { get; } = new List<IAsyncCompositeCommand>();

		/// <inheritdoc />
		public bool CanExecute(object parameter)
		{
			return Compositions.Count <= 0 || Compositions.TrueForAll(d => d.CanExecute(parameter));
		}

		/// <inheritdoc />
		public async void Execute(object parameter)
		{
			Compositions.ForEach(async (d) => await d.AllExecutingAsync(parameter));
			await Task.WhenAll(Compositions.Select(async (d) =>
			{
				await d.OnExecutingAsync(parameter);
				await d.ExecuteAsync(parameter);
				await d.OnExecutedAsync(parameter);
			}));
			Compositions.ForEach(async (d) => await d.AllExecutedAsync(parameter));
		}

		/// <inheritdoc />
		event EventHandler ICommand.CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
}