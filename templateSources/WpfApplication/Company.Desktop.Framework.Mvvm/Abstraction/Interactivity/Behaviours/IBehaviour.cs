using System;
using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours
{
	public interface IBehaviour : IDisposable
	{
		/// <summary>
		/// Order in which a behaviour will be executed
		/// </summary>
		int Priority { get; }

		IObservable<object> WhenExecuted { get; }
	}

	public interface IContextSettable<TContext>
	{
		void SetContext(TContext context);
	}

	public interface IAsyncBehaviour : IBehaviour
	{
		Task ExecuteAsync();
	}

	public interface IAsyncBehaviour<TContext> : IBehaviour, IContextSettable<TContext>
	{
		TContext Context { get; }

		Task ExecuteAsync();
	}

	public interface ISyncBehaviour : IBehaviour
	{
		void Execute();
	}

	public interface ISyncBehaviour<TContext> : IBehaviour, IContextSettable<TContext>
	{
		TContext Context { get; }

		void Execute();
	}
}