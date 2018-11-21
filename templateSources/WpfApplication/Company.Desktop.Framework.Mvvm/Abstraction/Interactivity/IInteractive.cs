﻿using System;
using System.Collections.Generic;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Abstraction.Interactivity
{
	public interface IInteractive : IDisposable
	{
		List<IBehaviour> Behaviours { get; }
	}
}