using System.Collections.Generic;
using System.Reflection;

namespace Company.Desktop.Framework.DependencyInjection
{
	public interface IInjectionAssemblyLoader
	{
		IEnumerable<Assembly> GetAssemblies();
	}
}