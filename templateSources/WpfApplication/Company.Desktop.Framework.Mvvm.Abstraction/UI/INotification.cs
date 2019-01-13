namespace Company.Desktop.Framework.Mvvm.Abstraction.UI
{
	public interface INotification
	{
		NotificationPosition Position { get; }

		NotificationTarget Target { get; }
	}
}