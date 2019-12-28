namespace Company.Desktop.Model.Entities
{
	public class SampleData
	{
		/// <inheritdoc />
		public SampleData()
		{
		}

		/// <inheritdoc />
		public SampleData(string value1, string value2)
		{
			Value1 = value1;
			Value2 = value2;
		}

		public string Value1 { get; set; }

		public string Value2 { get; set; }
	}
}
