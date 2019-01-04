using Company.Desktop.Framework.Mvvm.Abstraction.ViewModel;
using JetBrains.Annotations;

namespace Company.Desktop.Framework.Mvvm.ViewModel
{
	public class DefaultWindowViewModel : WindowViewModelBase
	{
		/// <inheritdoc />
		public DefaultWindowViewModel([NotNull] IWindowContentViewModel content) : base(content)
		{
		}
	}
}