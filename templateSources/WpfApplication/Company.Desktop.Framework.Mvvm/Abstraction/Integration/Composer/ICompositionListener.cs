namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Composer
{
	public interface ICompositionListener
	{
		void Execute(IViewCompositionContext context);
	}
}