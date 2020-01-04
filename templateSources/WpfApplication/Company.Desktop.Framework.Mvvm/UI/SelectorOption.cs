namespace Company.Desktop.Framework.Mvvm.UI
{
	public class SelectorOption<TValue>
	{
		public SelectorOption(TValue value, string description)
		{
			Value = value;
			Description = description;
		}

		public TValue Value { get; set; }

		public string Description { get; set; }

		public static implicit operator SelectorOption<TValue>((string text, TValue value) source)
		{
			return new SelectorOption<TValue>(source.value, source.text);
		}

		public static implicit operator SelectorOption<TValue>((TValue value, string text) source)
		{
			return new SelectorOption<TValue>(source.value, source.text);
		}
	}
}