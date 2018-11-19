using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Navigation;
using Company.Desktop.Framework.Mvvm.ViewModels;
using Company.Desktop.Models.Abstraction.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.ViewModels.Controls
{
	public class SampleDataOverviewViewModel : ContentViewModel
	{
		public int Count { get; }

		private ObservableCollection<SampleDataViewModel> _items = new ObservableCollection<SampleDataViewModel>();

		public ObservableCollection<SampleDataViewModel> Items
		{
			get => _items;
			set => SetValue(ref _items, value, nameof(Items));
		}

		public SampleDataOverviewViewModel(int count)
		{
			Count = count;
		}

		/// <inheritdoc />
		protected override async Task OnActivateAsync(IActivationContext context)
		{
			await Task.Delay(2000);
			var dataProvider = context.ServiceProvider.GetRequiredService<ISampleDataProvider>();
			Log.Debug($"ViewModel activated with parameter {Count}");

			var items = await dataProvider.GetAllAsync(Count);
			Items.Clear();
			foreach (var item in items)
			{
				Items.Add(new SampleDataViewModel(item));
			}
		}

		/// <inheritdoc />
		protected override void InitializeBehaviours()
		{
			
		}
	}
}