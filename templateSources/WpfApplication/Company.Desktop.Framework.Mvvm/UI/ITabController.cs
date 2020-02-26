using System;
using System.Threading.Tasks;

namespace Company.Desktop.Framework.Mvvm.UI
{
	public interface ITabController
	{
		bool IsOperational { get; }
		bool IsFocused(ITab model);
		void Insert(int index, ITab model);
		int Add(ITab model);
		void Remove(ITab model);
		void RemoveAt(int index);
		int FindIndex(ITab model);
		void Focus(ITab model);
		void FocusAt(int index);
		void FocusLast();
		int TabCount { get; }
	}

	public interface ITab
	{
		string Title { get; }
		IObservable<string> WhenTitleChanged { get; }
		bool Closable { get; }
		IObservable<bool> WhenClosableChanged { get; }
		Task<bool> TryCloseTabAsync();
	}
}