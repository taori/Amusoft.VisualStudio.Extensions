using System;
using System.Collections.Generic;
using Company.Desktop.Application.Views.Controls;
using Company.Desktop.Framework.Mvvm._sort;
using Company.Desktop.ViewModels.Controls;

namespace Company.Desktop.Application.Dependencies.Configuration.DataTemplate
{
	public class StaticTemplateMapper : IDataTemplateMapper
	{
		/// <inheritdoc />
		public IEnumerable<(Type viewModelType, Type viewType)> GetMappings(IEnumerable<Type> viewModelTypes, IEnumerable<Type> viewTypes)
		{
			yield return (typeof(SampleDataOverviewViewModel), typeof(SampleDataOverview));
		}
	}
}