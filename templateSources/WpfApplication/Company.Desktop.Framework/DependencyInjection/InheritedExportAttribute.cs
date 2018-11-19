using System;

namespace Company.Desktop.Framework.DependencyInjection
{
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true)]
	public class InheritedExportAttribute : Attribute
	{
		public Type ContractType { get; }

		public string ContactName { get; }

		public LifeTime LifeTime { get; set; } = LifeTime.PerRequest;

		public InheritedExportAttribute(Type contractType)
		{
			ContractType = contractType;
		}

		public InheritedExportAttribute(Type contractType, string contactName)
		{
			ContactName = contactName;
			ContractType = contractType;
		}
	}
}