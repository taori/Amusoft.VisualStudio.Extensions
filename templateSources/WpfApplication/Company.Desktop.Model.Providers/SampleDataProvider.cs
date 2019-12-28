using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Desktop.Model.Entities;
using Company.Desktop.Model.Providers.Abstraction;

namespace Company.Desktop.Model.Providers
{
	public class SampleDataProvider : ISampleDataProvider
	{
		/// <inheritdoc />
		public Task<IEnumerable<SampleData>> GetAllAsync(int count)
		{
			var items = new List<SampleData>();
			for (int i = 0; i < count; i++)
			{
				items.Add(new SampleData($"row {i} value 1", $"row {i} value 2"));
			}

			return Task.FromResult(items as IEnumerable<SampleData>);
		}
	}
}
