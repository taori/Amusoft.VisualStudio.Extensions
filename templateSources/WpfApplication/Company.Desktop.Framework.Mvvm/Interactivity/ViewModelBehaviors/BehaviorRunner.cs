using System.Linq;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class BehaviorRunner : IBehaviorRunner
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(BehaviorRunner));

		/// <inheritdoc />
		public void Execute<TArgument>(IBehaviorHost host, TArgument argument) where TArgument : IBehaviorArgument
		{
			if (host == null)
				return;

			Log.Debug($"Executing behaviours of [{host.GetType().FullName}] using [{argument.GetType().FullName}]");
			foreach (var behaviour in host.Behaviors.OrderByDescending(d => d.Priority))
			{
				if (behaviour is IBehavior<TArgument> castedBehaviour)
				{
					Log.Debug($"Executing [{behaviour.GetType().FullName}] using [{argument.GetType().FullName}]");
					castedBehaviour.Execute(argument);
				}
			}
		}

		/// <inheritdoc />
		public async Task ExecuteAsync<TArgument>(IBehaviorHost host, TArgument argument) where TArgument : IBehaviorArgument
		{
			if (host == null)
				return;

			Log.Debug($"Executing behaviours of [{host.GetType().FullName}].");
			foreach (var behaviour in host.Behaviors.OrderByDescending(d => d.Priority))
			{
				if (behaviour is IAsyncBehavior<TArgument> castedBehaviour)
				{
					Log.Debug($"Executing [{behaviour.GetType().FullName}] using [{argument.GetType().FullName}]");
					await castedBehaviour.ExecuteAsync(argument);
				}
			}
		}
	}
}