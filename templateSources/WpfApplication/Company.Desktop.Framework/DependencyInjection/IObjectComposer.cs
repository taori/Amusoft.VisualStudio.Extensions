namespace Company.Desktop.Framework.DependencyInjection
{
	public interface IObjectComposer
	{
		T Compose<T>()
			where T : class;
	}
}