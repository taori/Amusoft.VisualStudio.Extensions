using System;
using System.Collections.Generic;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;

namespace Company.Desktop.Application.Dependencies.Configuration.DataTemplate
{
	public class StaticTemplateMapper : IDataTemplateMapper
	{
		/// <inheritdoc />
		public IEnumerable<(Type viewModelType, Type viewType)> GetMappings(IEnumerable<Type> viewModelTypes, IEnumerable<Type> viewTypes)
		{
			// TODO: if you don't like conventional mapping you can statically map it here... but why would you do that? don't be a php coder.
			yield break;
		}
	}
}