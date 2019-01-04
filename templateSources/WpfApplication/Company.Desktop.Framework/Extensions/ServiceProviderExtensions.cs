using System;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.Extensions
{
	public static class ServiceProviderExtensions
	{
		public static bool TryGetService<T>(this IServiceProvider source, out T service)
		{
			service = source.GetService<T>();
			return service != null;
		}
	}
}