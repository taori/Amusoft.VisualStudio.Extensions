using Company.Desktop.Framework.Mvvm.ViewModel;
using Company.Desktop.Models.Abstraction.Entities;

namespace Company.Desktop.ViewModels.Controls
{
	public class SampleDataViewModel : ViewModelBase
	{
		public ISampleData Data { get; }

		public SampleDataViewModel(ISampleData data)
		{
			Data = data;
		}
	}
}