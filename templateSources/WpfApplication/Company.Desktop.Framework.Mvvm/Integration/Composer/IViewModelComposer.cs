namespace Company.Desktop.Framework.Mvvm.Integration.Composer
{
	public interface IViewModelComposer
	{
		T Compose<T>();
	}

	public class ViewModelComposer : IViewModelComposer
	{
		/// <inheritdoc />
		public T Compose<T>()
		{
			throw new System.NotImplementedException();
		}
	}
}