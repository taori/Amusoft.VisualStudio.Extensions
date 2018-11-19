using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Desktop.Models.Abstraction.Entities;
using Company.Desktop.Models.Abstraction.Providers;
using Company.Desktop.Models.Entities;

namespace Company.Desktop.Models.Providers
{
	public class SampleDataProvider : ISampleDataProvider
	{
		/// <inheritdoc />
		public Task<IEnumerable<ISampleData>> GetAllAsync(int count)
		{
			var items = new List<SampleData>();
			for (int i = 0; i < count; i++)
			{
				items.Add(new SampleData($"row {i} value 1", $"row {i} value 2"));
			}

			return Task.FromResult(items as IEnumerable<ISampleData>);
		}
	}
}
