namespace Company.Desktop.Framework.Mvvm.UI
{
	public interface INotification
	{
		NotificationPosition Position { get; }

		NotificationTarget Target { get; }
	}
}