using Company.Desktop.Framework.Mvvm.ViewModels;
using Company.Desktop.Models.Abstraction.Entities;
using Company.Desktop.ViewModels.Common;

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