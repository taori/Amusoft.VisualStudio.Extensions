﻿namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public class WindowArguments : ICoordinationArguments
	{
		/// <inheritdoc />
		public WindowArguments(string windowId)
		{
			WindowId = windowId;
		}

		public string WindowId { get; }
	}
}