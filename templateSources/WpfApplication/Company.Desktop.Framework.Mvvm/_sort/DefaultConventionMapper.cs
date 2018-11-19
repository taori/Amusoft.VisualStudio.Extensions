using System;
using System.Collections.Generic;

namespace Company.Desktop.Framework.Mvvm._sort
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