﻿using System;
using System.Collections.Generic;
using Company.Desktop.Framework.DependencyInjection;

namespace Company.Desktop.Framework.Mvvm.Integration.Environment
{
	[InheritedMefExport(typeof(IViewTypeSource), LifeTime = LifeTime.Singleton)]
	public interface IViewTypeSource
	{
		IEnumerable<Type> GetValues();
	}
}