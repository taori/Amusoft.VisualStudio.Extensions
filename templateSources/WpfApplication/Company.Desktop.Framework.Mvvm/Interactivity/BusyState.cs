using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using JetBrains.Annotations;

namespace Company.Desktop.Framework.Mvvm.Interactivity
{
	public class BusyState : IBusyState
	{
		private int _counter;

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

		public IDisposable Session()
		{
			return new BusyStateSession(this);
		}

		public IObservable<PropertyChangedEventArgs> WhenPropertyChanged => Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
			add => _propertyChanged += add,
			remove => _propertyChanged -= remove
			).Select(d => d.EventArgs);

		private event PropertyChangedEventHandler _propertyChanged;

		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add { _propertyChanged += value; }
			remove { _propertyChanged -= value; }
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			_propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
	}
}