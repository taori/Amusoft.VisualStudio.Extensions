using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Desktop.Framework.DataAccess;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Model.Entities;

namespace Company.Desktop.Model.Providers.Abstraction
{
	[InheritedMefExport(typeof(ISampleDataProvider))]
	public interface ISampleDataProvider : IDataProvider
	{
		Task<IEnumerable<SampleData>> GetAllAsync(int count);
	}
}