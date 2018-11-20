using System;
using System.Collections.Generic;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
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