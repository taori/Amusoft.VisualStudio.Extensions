﻿using System;

namespace WindowsService.Shell
{
	public class CompositionContext
	{
		/// <inheritdoc />
		public CompositionContext(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		public IServiceProvider ServiceProvider { get; }
	}
}