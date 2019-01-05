namespace Company.Desktop.Framework.Mvvm.Abstraction.UI
{
	public enum ViewTransitionType
	{
		Default,
		Normal,
		Up,
		Down,
		Right,
		RightReplace,
		Left,
		LeftReplace,
		Custom,
	}

	public interface IViewTransition
	{
		ViewTransitionType GetTransition();
	}
}