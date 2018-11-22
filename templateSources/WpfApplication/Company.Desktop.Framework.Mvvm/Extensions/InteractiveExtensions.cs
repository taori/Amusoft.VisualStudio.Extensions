using System;
using System.Linq;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Extensions
{
	public static class InteractiveExtensions
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(InteractiveExtensions));

		private static void AddLog(Type behaviourType, Type contextType)
		{
			if(contextType == null)
				Log.Debug($"Executing behaviours [{behaviourType}].");
			else
				Log.Debug($"Executing behaviours [{behaviourType}] with context [{contextType}].");
		}

		public static void ExecuteBehaviours<TBehaviour>(this IInteractive source) where TBehaviour : ISyncBehaviour
		{
			if (source == null)
				return;

			AddLog(typeof(TBehaviour), null);
			foreach (var behavior in source.Behaviours.OrderByDescending(d => d.Priority))
			{
				Log.Debug($"Executing behaviour {behavior.GetType()}");
				if (behavior is ISyncBehaviour behaviourCasted)
					behaviourCasted.Execute();
			}
		}

		public static void ExecuteBehaviours<TBehaviour, TContext>(this IInteractive source, TContext context) where TBehaviour : ISyncBehaviour<TContext>
		{
			if (source == null)
				return;

			AddLog(typeof(TBehaviour), typeof(TContext));
			foreach (var behavior in source.Behaviours.OrderByDescending(d => d.Priority))
			{
				Log.Debug($"Executing behaviour {behavior.GetType()}");
				if (behavior is ISyncBehaviour<TContext> behaviourCasted)
				{
					(behavior as IContextSettable<TContext>)?.SetContext(context);
					behaviourCasted.Execute();
				}
			}
		}

		public static async Task ExecuteBehavioursAsync<TBehaviour>(this IInteractive source) where TBehaviour : IAsyncBehaviour
		{
			if (source == null)
				return;

			AddLog(typeof(TBehaviour), null);
			foreach (var behavior in source.Behaviours.OrderByDescending(d => d.Priority))
			{
				Log.Debug($"Executing behaviour {behavior.GetType()}");
				if (behavior is IAsyncBehaviour casted)
					await casted.ExecuteAsync();
			}
		}

		public static async Task ExecuteBehavioursAsync<TBehaviour, TContext>(this IInteractive source, TContext context) where TBehaviour : IAsyncBehaviour<TContext>
		{
			if (source == null)
				return;

			AddLog(typeof(TBehaviour), typeof(TContext));
			foreach (var behavior in source.Behaviours.OrderByDescending(d => d.Priority))
			{
				Log.Debug($"Executing behaviour {behavior.GetType()}");
				if (behavior is IAsyncBehaviour<TContext> casted)
				{
					(behavior as IContextSettable<TContext>)?.SetContext(context);
					await casted.ExecuteAsync();
				}
			}
		}

	}
}