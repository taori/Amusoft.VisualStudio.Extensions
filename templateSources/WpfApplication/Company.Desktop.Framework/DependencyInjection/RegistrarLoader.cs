using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Company.Desktop.Framework.DependencyInjection
{
	internal class RegistrarLoader : IServiceRegistrar
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(RegistrarLoader));

		public IInjectionAssemblyLoader AssemblyLoader { get; }

		public RegistrarLoader(IInjectionAssemblyLoader assemblyLoader)
		{
			AssemblyLoader = assemblyLoader;
		}

		public void Register(IServiceCollection services)
		{
			Log.Debug($"Executing {nameof(RegistrarLoader)}");

			var conventions = new ConventionBuilder();
			conventions
				.ForTypesDerivedFrom<IServiceRegistrar>()
				.Export<IServiceRegistrar>()
				.Shared();

			var assemblies = AssemblyLoader.GetAssemblies().ToArray();
			Log.Debug($"Found {assemblies.Length} assemblies.");
			Log.Debug($"The following assemblies are used to load registrars:");
			foreach (var assembly in assemblies)
			{
				Log.Debug($"{assembly.FullName}, {assembly.Location}");
			}

			var configuration = new ContainerConfiguration()
				.WithServiceCollection(services)
				.WithAssemblies(assemblies, conventions);

			using (var container = configuration.CreateContainer())
			{
				var derivedRegistrars = GetDerivedRegistrars(container).ToArray();
				foreach (var registrar in derivedRegistrars)
				{
					Log.Debug($"Executing {registrar.GetType().FullName}");
					registrar.Register(services);
				}
			}
		}

		private IEnumerable<IServiceRegistrar> GetDerivedRegistrars(CompositionHost container)
		{
			return container
				.GetExports<IServiceRegistrar>()
				.Where(b => b.GetType() != typeof(RegistrarLoader));
		}
	}
}