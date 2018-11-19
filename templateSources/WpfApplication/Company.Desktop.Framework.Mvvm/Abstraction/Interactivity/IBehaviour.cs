using System;
using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IBehaviour
	{
		int ExecutionOrder { get; }

		event EventHandler Executed;
	}

	public interface IContextSettable<TContext>
	{
		TContext Context { set; }
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