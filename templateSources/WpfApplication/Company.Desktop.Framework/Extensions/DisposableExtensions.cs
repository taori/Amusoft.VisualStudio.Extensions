using System;
using System.Reactive.Disposables;

namespace Company.Desktop.Framework.Extensions
{
	public static class DisposableExtensions
	{
		public static void DisposeWith(this IDisposable source, CompositeDisposable with)
		{
			with.Add(source);
		}
	}
}