using System;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.ViewModelBehaviors
{

	public interface IContentChangingBehaviorContext : IBehaviorArgument
	{
		object OldViewModel { get; }
		object NewViewModel { get; }
		bool Cancelled { get; }
		IServiceProvider ServiceProvider { get; }
		void Cancel();
	}
}