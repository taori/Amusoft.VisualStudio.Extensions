namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public interface ICompositionListener
	{
		void Execute(IViewCompositionContext context);
	}
}