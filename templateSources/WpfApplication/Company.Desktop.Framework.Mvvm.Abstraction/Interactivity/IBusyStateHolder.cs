namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IBusyStateHolder
	{
		IBusyState LoadingState { get; }
	}
}