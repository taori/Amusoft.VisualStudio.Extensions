﻿using System.Collections.Generic;
using System.Reflection;
using Company.Desktop.Framework.DataAccess;
using Company.Desktop.Framework.DependencyInjection;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Models.Abstraction.Providers;
using Company.Desktop.Models.Providers;

namespace Company.Desktop.Application.Dependencies.Setup
{
	public class InjectionAssemblyLoader : IInjectionAssemblyLoader
	{
		/// <inheritdoc />
		public IEnumerable<Assembly> GetAssemblies()
		{
			yield return typeof(IDataProvider).Assembly;
			yield return typeof(ISampleDataProvider).Assembly;
			yield return typeof(SampleDataProvider).Assembly;
			yield return typeof(DependencyContainer).Assembly;
			yield return typeof(IRegionManager).Assembly;
		}
	}
}