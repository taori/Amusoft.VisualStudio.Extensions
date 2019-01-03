using System;
using System.Windows.Markup;

namespace Company.Desktop.Framework.Mvvm.Markup
{
	public class SystemTypeExtension : MarkupExtension
	{
		private object _value;

		public int Int
		{
			set => _value = value;
		}

		public double Double
		{
			set => _value = value;
		}

		public float Float
		{
			set => _value = value;
		}

		public bool Boolean
		{
			set => _value = value;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return _value;
		}
	}
}