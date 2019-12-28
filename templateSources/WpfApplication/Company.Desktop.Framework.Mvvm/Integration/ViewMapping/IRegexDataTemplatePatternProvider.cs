using System.Text.RegularExpressions;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public interface IRegexDataTemplatePatternProvider
	{
		Regex ViewModelTypeRegex { get; }
		Regex ViewTypeRegex { get; }
	}
}