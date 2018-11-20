using System;
using System.Collections.Generic;
using System.Linq;
using Company.Desktop.Application.Views.Windows;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;

namespace Company.Desktop.Application.Dependencies.Configuration.DataTemplate
{
	public class LocalViewTypeSource : IViewTypeSource
	{
		/// <inheritdoc />
		public IEnumerable<Type> GetValues()
		{
			return typeof(DefaultWindow).Assembly.ExportedTypes.Where(d => d.FullName.StartsWith("Company.Desktop.Application.Views"));
		}
	}
}