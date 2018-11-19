using System;
using System.Collections.Generic;
using Company.Desktop.Framework.Mvvm._sort;

namespace Company.Desktop.Framework.Mvvm.ViewModel.Mapping
{
	public class DefaultConventionMapper : IDataTemplateMapper
	{
		/// <inheritdoc />
		public IEnumerable<(Type viewModelType, Type viewType)> GetMappings(IEnumerable<Type> viewModelTypes, IEnumerable<Type> viewTypes)
		{
			yield break;
		}
	}
}