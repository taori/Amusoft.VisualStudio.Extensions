namespace Company.Desktop.Framework.Mvvm.Abstraction.Integration.Environment
{
	public interface ISettingsStorage
	{
		bool TryGetValue<T>(string key, out T value) where T : class;
		void UpdateValue<T>(string key, T value) where T : class;
		void Save();
	}
}