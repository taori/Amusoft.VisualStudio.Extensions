using System;

namespace Company.Desktop.Framework.Mvvm.UI
{
	public class ProgressController : IDisposable
	{
		public IProgressController ProgressAdapter { get; }

		public ProgressController(IProgressController progressAdapter)
		{
			ProgressAdapter = progressAdapter;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			ProgressAdapter?.Dispose();
		}
	}
}