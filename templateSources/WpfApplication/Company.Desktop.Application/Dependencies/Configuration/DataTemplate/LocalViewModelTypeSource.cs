using System;
using System.Collections.Generic;
using System.Linq;
using Company.Desktop.Framework.Mvvm.Integration.Environment;

namespace Company.Desktop.Application.Dependencies.Configuration.DataTemplate
{
	public class LocalViewModelTypeSource : IViewModelTypeSource
	{
		/// <inheritdoc />
		public IEnumerable<Type> GetValues()
		{
			return typeof(ViewModels.Common.RegionNames).Assembly.ExportedTypes.Where(d => d.FullName.StartsWith("Company.Desktop.ViewModels"));
		}
	}
}