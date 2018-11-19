using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Company.Desktop.Framework.Mvvm.Properties;
using JetBrains.Annotations;

namespace Company.Desktop.Framework.Mvvm
{
	public class BusyState : INotifyPropertyChanged
	{

		private int _counter = 0;

		public bool IsBusy
		{
			get { return _counter > 0; }
			set { }
		}

		public void Increment()
		{
			_counter++;
			OnPropertyChanged(nameof(IsBusy));
		}

		public void Decrement()
		{
			_counter--;
			OnPropertyChanged(nameof(IsBusy));
		}

		private class BusyStateSession : IDisposable
		{
			public BusyState State { get; }

			public BusyStateSession(BusyState state)
			{
				State = state;
				state.Increment();
			}

			/// <inheritdoc />
			public void Dispose()
			{
				State.Decrement();
			}
		}

		public IDisposable Session()
		{
			return new BusyStateSession(this);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}