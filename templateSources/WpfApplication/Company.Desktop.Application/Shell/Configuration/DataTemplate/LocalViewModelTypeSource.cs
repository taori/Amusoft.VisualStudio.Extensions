using System;
using System.Collections.Generic;
using System.Linq;
using Company.Desktop.Framework.Mvvm;

namespace Company.Desktop.Application.Shell.Configuration.DataTemplate
{
	public class LocalViewModelTypeSource : IViewModelTypeSource
	{
		/// <inheritdoc />
		public IEnumerable<Type> GetValues()
		{
			return typeof(Company.Desktop.ViewModels.Common.RegionNames).Assembly.ExportedTypes.Where(d => d.FullName.StartsWith("Company.Desktop.ViewModels"));
		}
	}
}