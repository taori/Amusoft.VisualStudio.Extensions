using AutoMapper;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using Company.Desktop.ViewModels.Services;

namespace Company.Desktop.Application.Dependencies.Configuration
{
	public class ApplicationSettings : IApplicationSettings
	{
		private const string StorageKey = "LocalApplicationSettings";
		private readonly ISettingsStorage _storage;

		private static readonly ApplicationSettings Default = new ApplicationSettings();

		/// <summary>
		/// Required for DI constructor. Do not use.
		/// </summary>
		public ApplicationSettings() { }

		public ApplicationSettings(ISettingsStorage storage)
		{
			_storage = storage;

			if (!_storage.TryGetValue<ApplicationSettings>(StorageKey, out var oldSettings))
			{
				oldSettings = Default;
			}

			CreateMapper().Map(oldSettings, this);
		}

		private static Mapper CreateMapper()
		{
			return new Mapper(new MapperConfiguration(config =>
			{
				config.CreateMap<ApplicationSettings, ApplicationSettings>();
			}));
		}

		public bool FocusTabOnCreate { get; set; } = true;
		public bool FocusTabOnOpen { get; set; } = true;

		public void Update()
		{
			_storage.UpdateValue(StorageKey, this);
			_storage.Save();
		}
	}
}