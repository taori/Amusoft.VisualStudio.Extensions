using System;
using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors
{
	public interface IBehaviorArgument
	{

	}

	public interface IBehavior : IDisposable
	{
		/// <summary>
		/// Order in which a behaviour will be executed
		/// </summary>
		int Priority { get; }

		IObservable<object> WhenExecuted { get; }
	}

	public interface IBehavior<in TArgument> : IBehavior
		where TArgument : IBehaviorArgument
	{
		void Execute(TArgument context);
	}

	public interface IAsyncBehavior<in TArgument> : IBehavior
		where TArgument : IBehaviorArgument
	{
		Task ExecuteAsync(TArgument context);
	}

	public interface IBehaviorRunner
	{
		void Execute<TArgument>(IBehaviorHost host, TArgument argument) where TArgument : IBehaviorArgument;
		Task ExecuteAsync<TArgument>(IBehaviorHost host, TArgument argument) where TArgument : IBehaviorArgument;
	}
}