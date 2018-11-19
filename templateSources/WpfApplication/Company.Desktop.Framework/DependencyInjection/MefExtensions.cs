using System.Collections.Generic;
using System.Composition.Convention;
using System.Linq;
using System.Reflection;

namespace Company.Desktop.Framework.DependencyInjection
{
	public static class MefExtensions
	{
		public static void ImportFromAttributes(this ConventionBuilder convention, IEnumerable<Assembly> assemblies)
		{
			var exportedTypes = assemblies.SelectMany(d => d.ExportedTypes).ToList();
			var serviceTypes = exportedTypes.Where(d => d.IsDefined(typeof(InheritedExportAttribute))).Select(s => new { type = s, attributes = s.GetCustomAttributes<InheritedExportAttribute>() }).ToList();
			var creationPolicyByServiceType = serviceTypes.ToDictionary(key => key.type, val => new { creationPolicyAttr = val.type.GetCustomAttribute<PartCreationPolicyAttribute>(), exportAttr = val.attributes });

			PartConventionBuilder partBuilder;
			foreach (var implementationType in exportedTypes)
			{
				partBuilder = null;
				if (implementationType.IsAbstract || implementationType.IsInterface || !implementationType.IsClass)
					continue;

				PartCreationPolicyAttribute sharePolicy = null;
				foreach (var serviceType in creationPolicyByServiceType)
				{
					if (!serviceType.Key.IsAssignableFrom(implementationType))
						continue;

					partBuilder = partBuilder ?? convention.ForType(implementationType);
					foreach (var attribute in serviceType.Value.exportAttr)
					{
						if (string.IsNullOrEmpty(attribute.ContactName))
						{
							partBuilder.Export(config => config.AsContractType(attribute.ContractType));
						}
						else
						{
							partBuilder.Export(config => config.AsContractType(attribute.ContractType).AsContractName(attribute.ContactName));
						}
					}

					sharePolicy = serviceType.Value.creationPolicyAttr;
				}


				if (sharePolicy != null)
				{
					if (!string.IsNullOrEmpty(sharePolicy.SharingBoundary) && sharePolicy.SharingBoundary != PartCreationPolicyAttribute.DefaultShared)
					{
						partBuilder.Shared(sharePolicy.SharingBoundary);
						continue;
					}
					if (sharePolicy.SharingBoundary == PartCreationPolicyAttribute.DefaultShared)
					{
						partBuilder.Shared();
						continue;
					}
				}
			}
		}
	}
}