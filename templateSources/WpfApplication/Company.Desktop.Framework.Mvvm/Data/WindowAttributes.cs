namespace Company.Desktop.Framework.Mvvm.Data
{
	public class WindowAttributes
	{
		/// <inheritdoc />
		public WindowAttributes(double width, double height, double left, double top)
		{
			Width = width;
			Height = height;
			Left = left;
			Top = top;
		}

		/// <inheritdoc />
		public WindowAttributes()
		{
		}

		public double Width { get; set; }

		public double Height { get; set; }

		public double Left { get; set; }

		public double Top { get; set; }
	}
}