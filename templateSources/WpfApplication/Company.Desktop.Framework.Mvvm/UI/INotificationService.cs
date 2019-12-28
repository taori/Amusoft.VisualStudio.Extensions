using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.UI
{
	[InheritedMefExport(typeof(INotificationService))]
	public interface INotificationService
	{
		void Display(INotification notification);
	}
}