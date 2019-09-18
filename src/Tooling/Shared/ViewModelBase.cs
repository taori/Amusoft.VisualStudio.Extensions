using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Tooling.Properties;

namespace Tooling.Shared
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			_whenPropertyChanged.OnNext(propertyName);
		}

		protected virtual bool SetValue<T>(ref T field, T value, string propertyName)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
				return false;

			if (field != null && field is INotifyPropertyChanged unsub)
			{
				unsub.PropertyChanged -= ValuePropertyChanged(propertyName);
			}

			field = value;

			if (field != null && field is INotifyPropertyChanged sub)
			{
				sub.PropertyChanged += ValuePropertyChanged(propertyName);
			}

			OnPropertyChanged(propertyName);
			return true;
		}

		private PropertyChangedEventHandler ValuePropertyChanged(string propertyName)
		{
			return (sender, args) => { OnPropertyChanged(propertyName); };
		}

		private Subject<string> _whenPropertyChanged = new Subject<string>();
		public IObservable<string> WhenPropertyChanged => _whenPropertyChanged;
	}
}