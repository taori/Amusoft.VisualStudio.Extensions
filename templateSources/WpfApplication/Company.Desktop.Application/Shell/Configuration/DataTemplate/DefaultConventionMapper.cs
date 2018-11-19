using System;
using System.Collections.Generic;
using Company.Desktop.Framework.Mvvm;

namespace Company.Desktop.Application.Shell.Configuration.DataTemplate
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