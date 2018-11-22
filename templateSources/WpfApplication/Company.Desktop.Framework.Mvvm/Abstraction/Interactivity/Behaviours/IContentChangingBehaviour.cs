using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours
{
	public interface IContentChangingBehaviour : IAsyncBehaviour<IContentChangingBehaviourContext>
	{

	}

	public interface IContentChangingBehaviourContext : IBehaviourArgument
	{
		object OldViewModel { get; }
		object NewViewModel { get; }
		bool Cancelled { get; }
		IServiceProvider ServiceProvider { get; }
		void Cancel();
	}
}