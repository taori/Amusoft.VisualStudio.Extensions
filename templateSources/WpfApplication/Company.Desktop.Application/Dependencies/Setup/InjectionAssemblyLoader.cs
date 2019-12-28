using System.Collections.Generic;
using System.Reflection;
using Company.Desktop.Framework.Controls;
using Company.Desktop.Framework.DataAccess;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;
using Company.Desktop.Model.Entities;
using Company.Desktop.Model.Providers;
using Company.Desktop.Shared.Utility;

namespace Company.Desktop.Application.Dependencies.Setup
{
	public class InjectionAssemblyLoader : IInjectionAssemblyLoader
	{
		/// <inheritdoc />
		public IEnumerable<Assembly> GetAssemblies()
		{
			yield return typeof(IDataProvider).Assembly;
			yield return typeof(SampleData).Assembly;
			yield return typeof(SampleDataProvider).Assembly;
			yield return typeof(DependencyContainer).Assembly;
			yield return typeof(IRegionManager).Assembly;
			yield return typeof(RegionManager).Assembly;
			yield return typeof(FileHelper).Assembly;
			yield return typeof(OverlayPanel).Assembly;
		}
	}
}