namespace Company.Desktop.Framework.Mvvm.Integration.Construction
{
	public static class ParameterExtensions
	{
		public static TModel WithParameters<TModel, T1>(this TModel source, T1 v1)
			where TModel : IParameter<T1>
		{
			source.Using(v1);
			return source;
		}

		public static TModel WithParameters<TModel, T1, T2>(this TModel source, T1 v1, T2 v2)
			where TModel : IParameter<T1, T2>
		{
			source.Using(v1, v2);
			return source;
		}

		public static TModel WithParameters<TModel, T1, T2, T3>(this TModel source, T1 v1, T2 v2, T3 v3)
			where TModel : IParameter<T1, T2, T3>
		{
			source.Using(v1, v2, v3);
			return source;
		}

		public static TModel WithParameters<TModel, T1, T2, T3, T4>(this TModel source, T1 v1, T2 v2, T3 v3, T4 v4)
			where TModel : IParameter<T1, T2, T3, T4>
		{
			source.Using(v1, v2, v3, v4);
			return source;
		}
	}
}