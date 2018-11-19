using Company.Desktop.Framework.Mvvm.Interactivity;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IBusyStateHolder
	{
		BusyState LoadingState { get; }
	}
}