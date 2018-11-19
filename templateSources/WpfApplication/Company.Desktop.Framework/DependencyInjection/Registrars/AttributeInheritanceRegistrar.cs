
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Company.Desktop.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.DependencyInjection.Registrars
{
	public class AttributeInheritanceRegistrar : IServiceRegistrar
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(AttributeInheritanceRegistrar));

		public IInjectionAssemblyLoader AssemblyLoader { get; }

		public AttributeInheritanceRegistrar(IInjectionAssemblyLoader assemblyLoader)
		{
			AssemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
		}

		/// <inheritdoc />
		public void Register(IServiceCollection services)
		{
			var exportedTypes = new HashSet<Type>(AssemblyLoader.GetAssemblies().SelectMany(d => d.ExportedTypes));
			var typesWithInheritance = GetTypesWithInheritedExports(exportedTypes);
			var inheritorsByType = typesWithInheritance.ToDictionary(d => d.type, d => d.attributes);
			RegisterImplementorsForTypes(services, exportedTypes, inheritorsByType);
		}

		private void RegisterImplementorsForTypes(IServiceCollection services, HashSet<Type> exportedTypes, Dictionary<Type, IEnumerable<InheritedExportAttribute>> inheritorsByType)
		{
			foreach (var exportedType in exportedTypes)
			{
				foreach (var @interface in exportedType.GetInterfaces())
				{
					if (inheritorsByType.TryGetValue(@interface, out var exportAttributes))
					{
						if (@interface.IsAssignableFrom(exportedType) && IsRegisterableType(exportedType))
						{
							foreach (var attribute in exportAttributes)
							{
								RegisterByAttribute(services, attribute.ContractType, exportedType, attribute.LifeTime);
							}
						}
					}
				}

				foreach (var hierarchyType in exportedType.GetTypeHierarchy())
				{
					if(hierarchyType.IsAbstract)
						continue;

					if (inheritorsByType.TryGetValue(hierarchyType, out var exportAttributes))
					{
						foreach (var attribute in exportAttributes)
						{
							RegisterByAttribute(services, attribute.ContractType, exportedType, attribute.LifeTime);
						}
					}
				}
			}
		}

		private bool IsRegisterableType(Type exportedType)
		{
			return !exportedType.IsInterface
			       && !exportedType.IsAbstract;
		}

		private static void RegisterByAttribute(IServiceCollection services, Type serviceType, Type implementationType, LifeTime lifeTime)
		{
			Log.Debug($"Registering [{lifeTime}] [{serviceType.FullName}] -> [{implementationType.FullName}].");
			switch (lifeTime)
			{
				case LifeTime.PerRequest:
					services.AddTransient(serviceType, implementationType);
					break;
				case LifeTime.PerScope:
					services.AddScoped(serviceType, implementationType);
					break;
				case LifeTime.Singleton:
					services.AddSingleton(serviceType, implementationType);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static List<(Type type, IEnumerable<InheritedExportAttribute> attributes)> GetTypesWithInheritedExports(HashSet<Type> exportedTypes)
		{
			var serviceTypes = exportedTypes
				.Where(d => d.IsDefined(typeof(InheritedExportAttribute)))
				.Select(s => (s, s.GetCustomAttributes<InheritedExportAttribute>(true)))
				.ToList();

			return serviceTypes;
		}
	}
}