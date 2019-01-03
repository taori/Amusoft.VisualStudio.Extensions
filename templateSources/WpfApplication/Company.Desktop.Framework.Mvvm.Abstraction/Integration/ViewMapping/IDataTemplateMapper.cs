using System;
using System.Collections.Generic;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping
{
	[InheritedExport(typeof(IDataTemplateMapper), LifeTime = LifeTime.Singleton)]
	public interface IDataTemplateMapper
	{
		IEnumerable<(Type viewModelType, Type viewType)> GetMappings(IEnumerable<Type> viewModelTypes, IEnumerable<Type> viewTypes);
	}
}