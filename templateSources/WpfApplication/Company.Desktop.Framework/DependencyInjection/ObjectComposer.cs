using System;
using System.Linq;
using System.Reflection;

namespace Company.Desktop.Framework.DependencyInjection
{
	public class ObjectComposer : IObjectComposer
	{
		private readonly IServiceProvider _serviceProvider;

		public ObjectComposer(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public T Compose<T>() 
			where T : class
		{
			var constructor = GetConstructor<T>();
			var composed = constructor.Invoke(GetParameterInstances(constructor));
			if (composed is ICompositionCompleted compositionCompleted)
				compositionCompleted.Complete();

			return composed as T;
		}

		private object[] GetParameterInstances(ConstructorInfo constructor)
		{
			var parameterInfos = constructor.GetParameters();
			var parameterValues = new object[parameterInfos.Length];
			var index = 0;
			foreach (var parameter in parameterInfos)
			{
				parameterValues[index++] = _serviceProvider.GetService(parameter.ParameterType);
			}

			return parameterValues;
		}

		private static ConstructorInfo GetConstructor<T>()
		{
			var constructors = typeof(T).GetConstructors();
			if (constructors.Length > 0)
			{
				if (constructors.Length > 1)
				{
					var mapping = constructors.Select(constructor =>
						(
							attribute: CustomAttributeExtensions.GetCustomAttribute<ComposerImportAttribute>((MemberInfo) constructor),
							constructor
						)
					);

					var declaringConstructor = mapping.FirstOrDefault(d => d.attribute != null);
					if (declaringConstructor == default)
					{
						throw new System.NotSupportedException(
							$"There is more than one constructor - mark one with the attribute [{nameof(ComposerImportAttribute)}].");
					}

					return declaringConstructor.constructor;
				}
				else
				{
					return constructors[0];
				}
			}
			else
			{
				throw new System.NotSupportedException("No public constructor has been found.");
			}
		}
	}
}