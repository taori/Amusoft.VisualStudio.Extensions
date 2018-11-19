namespace Company.Desktop.Framework.Mvvm._sort
{
	public class WindowArguments : ICoordinationArguments
	{
		/// <inheritdoc />
		public WindowArguments(string windowId)
		{
			WindowId = windowId;
		}

		public string WindowId { get; }
	}
}