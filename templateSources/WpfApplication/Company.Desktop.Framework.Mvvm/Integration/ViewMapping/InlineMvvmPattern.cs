using System.Text.RegularExpressions;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public class InlineMvvmPattern : IRegexDataTemplatePatternProvider
	{
		/// <inheritdoc />
		public InlineMvvmPattern(Regex viewModelTypeRegex, Regex viewTypeRegex)
		{
			ViewModelTypeRegex = viewModelTypeRegex;
			ViewTypeRegex = viewTypeRegex;
		}

		/// <inheritdoc />
		public Regex ViewModelTypeRegex { get; }

		/// <inheritdoc />
		public Regex ViewTypeRegex { get; } 
	}
}