
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Desktop.Framework.DataAccess;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Models.Abstraction.Entities;

namespace Company.Desktop.Models.Abstraction.Providers
{
	[InheritedExport(typeof(ISampleDataProvider))]
	public interface ISampleDataProvider : IDataProvider
	{
		Task<IEnumerable<ISampleData>> GetAllAsync(int count);
	}
}