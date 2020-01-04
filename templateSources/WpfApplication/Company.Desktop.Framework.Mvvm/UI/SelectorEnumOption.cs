using System;

namespace Company.Desktop.Framework.Mvvm.UI
{
	public class SelectorEnumOption<TValue> where TValue : struct, Enum
	{
		public SelectorEnumOption(TValue value, string description)
		{
			Value = value;
			Description = description;
		}

		public TValue Value { get; set; }

		public string Description { get; set; }

		public static implicit operator SelectorEnumOption<TValue>((string text, TValue value) source)
		{
			return new SelectorEnumOption<TValue>(source.value, source.text);
		}

		public static implicit operator SelectorEnumOption<TValue>((TValue value, string text) source)
		{
			return new SelectorEnumOption<TValue>(source.value, source.text);
		}
	}
}