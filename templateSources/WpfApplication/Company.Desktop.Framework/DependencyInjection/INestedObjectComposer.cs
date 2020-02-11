using System;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Desktop.Framework.DependencyInjection
{
	/// <summary>
	/// <para>Composer API with the ability to compose an object with an alternate set of dependencies, based on the dependencies the composer was built on.</para>
	/// <para>The original service collection remains unchanged.</para>
	/// </summary>
	public interface INestedObjectComposer
	{
		/// <summary>
		/// Modifies a cloned serviceCollection prior to composing the given object with it
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="beforeCompose"></param>
		/// <returns></returns>
		T Compose<T>(Action<IServiceCollection> beforeCompose = null)
			where T : class;
	}
}