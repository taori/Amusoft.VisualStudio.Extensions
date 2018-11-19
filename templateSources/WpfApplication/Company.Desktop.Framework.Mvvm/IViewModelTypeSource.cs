﻿using System;
using System.Collections.Generic;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm
{
	[InheritedExport(typeof(IViewModelTypeSource), LifeTime = LifeTime.Singleton)]
	public interface IViewModelTypeSource
	{
		IEnumerable<Type> GetValues();
	}
}