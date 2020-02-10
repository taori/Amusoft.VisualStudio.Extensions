namespace Company.Desktop.Framework.Mvvm.Integration.Construction
{
	public interface IParameter { }

	public interface IParameter<T1> : IParameter
	{
		void Using(T1 p1);
	}

	public interface IParameter<T1, T2> : IParameter
	{
		void Using(T1 p1, T2 p2);
	}

	public interface IParameter<T1, T2, T3> : IParameter
	{
		void Using(T1 p1, T2 p2, T3 p3);
	}

	public interface IParameter<T1, T2, T3, T4> : IParameter
	{
		void Using(T1 p1, T2 p2, T3 p3, T4 p4);
	}
}