using Company.Desktop.Framework.Mvvm.ViewModel;
using Company.Desktop.Model.Entities;

namespace Company.Desktop.ViewModels.Controls
{
	public class SampleDataViewModel : ViewModelBase
	{
		public SampleData Data { get; }

		public SampleDataViewModel(SampleData data)
		{
			Data = data;
		}
	}
}