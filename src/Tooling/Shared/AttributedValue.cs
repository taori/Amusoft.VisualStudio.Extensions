namespace Tooling.Shared
{
	public class AttributedValue<T> : ViewModelBase
	{
		private string _message;

		public string Message
		{
			get => _message;
			set => SetValue(ref _message, value, nameof(Message));
		}

		private T _value;

		public T Value
		{
			get => _value;
			set => SetValue(ref _value, value, nameof(Value));
		}

		public static implicit operator AttributedValue<T>(T value)
		{
			return new AttributedValue<T>() {_value = value};
		}
	}
}