using System.Collections.Generic;
using System.Linq;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration;
using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel.Mapping;
using Company.Desktop.Framework.Mvvm._sort;

namespace Company.Desktop.Framework.Mvvm.Integration
{
	public class DataTemplateConfigurationRunner : IConfigurationRunner
	{
		public IEnumerable<IViewModelTypeSource> ViewModelTypeSources { get; }
		public IEnumerable<IViewTypeSource> ViewTypeSources { get; }
		public IEnumerable<IDataTemplateMapper> MappingProviders { get; }

		/// <inheritdoc />
		public DataTemplateConfigurationRunner(IEnumerable<IViewModelTypeSource> viewModelTypeSources, IEnumerable<IViewTypeSource> viewTypeSources, IEnumerable<IDataTemplateMapper> mappingProviders)
		{
			ViewModelTypeSources = viewModelTypeSources;
			ViewTypeSources = viewTypeSources;
			MappingProviders = mappingProviders;
		}

		/// <inheritdoc />
		public void Execute()
		{
			var viewModelTypes = ViewModelTypeSources.SelectMany(s => s.GetValues()).ToArray();
			var viewTypes = ViewTypeSources.SelectMany(s => s.GetValues()).ToArray();
			var manager = new DataTemplateManager();
			foreach (var mappingProvider in MappingProviders)
			{
				foreach (var tuple in mappingProvider.GetMappings(viewModelTypes, viewTypes))
				{
					manager.RegisterDataTemplate(tuple.viewModelType, tuple.viewType);
				}
			}
		}
	}
}