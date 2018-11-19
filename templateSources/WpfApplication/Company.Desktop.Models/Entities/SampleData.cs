using Company.Desktop.Models.Abstraction.Entities;

namespace Company.Desktop.Models.Entities
{
	public class SampleData : ISampleData
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
