﻿using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment
{
	[InheritedMefExport(typeof(IConfigurationRunner), LifeTime = LifeTime.Singleton)]
	public interface IConfigurationRunner
	{
		void Execute();
	}
}