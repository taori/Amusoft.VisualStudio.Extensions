using System;
using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours
{
	public interface IBehaviourArgument
	{

	}

	public interface IBehaviour : IDisposable
	{
		/// <summary>
		/// Order in which a behaviour will be executed
		/// </summary>
		int Priority { get; }

		IObservable<object> WhenExecuted { get; }
	}

	public interface IBehaviour<in TArgument> : IBehaviour
		where TArgument : IBehaviourArgument
	{
		void Execute(TArgument context);
	}

	public interface IAsyncBehaviour<in TArgument> : IBehaviour
		where TArgument : IBehaviourArgument
	{
		Task ExecuteAsync(TArgument context);
	}

	public interface IBehaviourRunner
	{
		void Execute<TArgument>(IBehaviourHost host, TArgument argument) where TArgument : IBehaviourArgument;
		Task ExecuteAsync<TArgument>(IBehaviourHost host, TArgument argument) where TArgument : IBehaviourArgument;
	}
}