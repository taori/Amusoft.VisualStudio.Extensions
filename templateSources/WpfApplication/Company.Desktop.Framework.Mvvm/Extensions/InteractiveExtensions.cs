using System.Linq;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Extensions
{
	public static class InteractiveExtensions
	{
		public static void ExecuteBehaviours<TBehaviour>(this IInteractive source) where TBehaviour : ISyncBehaviour
		{
			if (source == null)
				return;

			foreach (var behavior in source.Behaviours.OrderBy(d => d.ExecutionOrder))
			{
				if (behavior is ISyncBehaviour behaviourCasted)
					behaviourCasted.Execute();
			}
		}

		public static void ExecuteBehaviours<TBehaviour, TContext>(this IInteractive source, TContext context) where TBehaviour : ISyncBehaviour<TContext>
		{
			if (source == null)
				return;

			foreach (var behavior in source.Behaviours.OrderBy(d => d.ExecutionOrder))
			{
				if (behavior is ISyncBehaviour<TContext> behaviourCasted)
				{
					if (behavior is IContextSettable<TContext> settable)
						settable.Context = context;
					behaviourCasted.Execute();
				}
			}
		}

		public static async Task ExecuteBehavioursAsync<TBehaviour>(this IInteractive source) where TBehaviour : IAsyncBehaviour
		{
			if (source == null)
				return;

			foreach (var behavior in source.Behaviours.OrderBy(d => d.ExecutionOrder))
			{
				if (behavior is IAsyncBehaviour casted)
					await casted.ExecuteAsync();
			}
		}

		public static async Task ExecuteBehavioursAsync<TBehaviour, TContext>(this IInteractive source, TContext context) where TBehaviour : IAsyncBehaviour<TContext>
		{
			if (source == null)
				return;

			foreach (var behavior in source.Behaviours.OrderBy(d => d.ExecutionOrder))
			{
				if (behavior is IAsyncBehaviour<TContext> casted)
				{
					if (behavior is IContextSettable<TContext> settable)
						settable.Context = context;
					await casted.ExecuteAsync();
				}
			}
		}

	}
}