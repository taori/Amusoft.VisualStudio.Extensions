using System.Linq;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class BehaviourRunner : IBehaviourRunner
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(BehaviourRunner));

		/// <inheritdoc />
		public void Execute<TArgument>(IBehaviourHost host, TArgument argument) where TArgument : IBehaviourArgument
		{
			if (host == null)
				return;

			Log.Debug($"Executing behaviours of [{host.GetType().FullName}] using [{argument.GetType().FullName}]");
			foreach (var behaviour in host.Behaviours.OrderByDescending(d => d.Priority))
			{
				if (behaviour is IBehaviour<TArgument> castedBehaviour)
				{
					Log.Debug($"Executing [{behaviour.GetType().FullName}] using [{argument.GetType().FullName}]");
					castedBehaviour.Execute(argument);
				}
			}
		}

		/// <inheritdoc />
		public async Task ExecuteAsync<TArgument>(IBehaviourHost host, TArgument argument) where TArgument : IBehaviourArgument
		{
			if (host == null)
				return;

			Log.Debug($"Executing behaviours of [{host.GetType().FullName}].");
			foreach (var behaviour in host.Behaviours.OrderByDescending(d => d.Priority))
			{
				if (behaviour is IAsyncBehaviour<TArgument> castedBehaviour)
				{
					Log.Debug($"Executing [{behaviour.GetType().FullName}] using [{argument.GetType().FullName}]");
					await castedBehaviour.ExecuteAsync(argument);
				}
			}
		}
	}
}