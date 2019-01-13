using System;

namespace Company.Desktop.Framework.DependencyInjection
{
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true)]
	public class InheritedMefExportAttribute : Attribute
	{
		public Type ContractType { get; }

		public string ContactName { get; }

		public LifeTime LifeTime { get; set; } = LifeTime.PerRequest;

		public InheritedMefExportAttribute(Type contractType)
		{
			ContractType = contractType;
		}

		public InheritedMefExportAttribute(Type contractType, string contactName)
		{
			ContactName = contactName;
			ContractType = contractType;
		}
	}
}