using System.Collections.Generic;
using System.Linq;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using Company.Desktop.Framework.Mvvm.Integration.ViewMapping;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.Environment
{
	public class DataTemplateConfigurationRunner : IConfigurationRunner
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(DataTemplateConfigurationRunner));

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
			Log.Debug($"Registering DataTemplates using {MappingProviders.Count()} {nameof(MappingProviders)}.");

			var viewModelTypes = ViewModelTypeSources.SelectMany(s => s.GetValues()).ToArray();
			var viewTypes = ViewTypeSources.SelectMany(s => s.GetValues()).ToArray();
			var manager = new DataTemplateManager();
			foreach (var mappingProvider in MappingProviders)
			{
				Log.Debug($"Registering mappings using {mappingProvider.GetType().FullName}.");
				foreach (var tuple in mappingProvider.GetMappings(viewModelTypes, viewTypes))
				{
					manager.RegisterDataTemplate(tuple.viewModelType, tuple.viewType);
				}
			}
		}
	}
}