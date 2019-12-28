using System;
using System.Collections.Generic;
using Company.Desktop.Application.Resources;
using Company.Desktop.Framework.Mvvm.Integration.Environment;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace Company.Desktop.Application.Dependencies
{
	public class SettingsStorage : ISettingsStorage
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(SettingsStorage));

		private Dictionary<string, object> _values;

		/// <inheritdoc />
		public bool TryGetValue<T>(string key, out T value) where T : class
		{
			if (_values == null)
			{
				lock (this)
				{
					try
					{
						_values = JsonConvert.DeserializeObject<Dictionary<string, object>>(ApplicationSettings.Default.SettingsStorage) ?? new Dictionary<string, object>();
					}
					catch (Exception e)
					{
						Log.Error(e);
					}
				}
			}

			if (_values == null)
			{
				Log.Error($"Value is still null - it should not be.");
				value = default(T);
				return false;
			}

			try
			{
				if (_values.TryGetValue(key, out var extracted))
				{
					Log.Debug($"Found a value for {key}.");
					if (extracted is T typed)
					{
						Log.Debug($"Value matches expected type [{typeof(T).FullName}].");
						value = typed;
						return true;
					}
					if (extracted is JObject jObject && jObject.ToObject<T>() is T jTyped)
					{
						Log.Debug($"jObject matches expected type [{typeof(T).FullName}].");
						value = jTyped;
						return true;
					}
					else
					{
						Log.Debug($"Type is [{extracted.GetType().FullName}] instead of [{typeof(T).FullName}].");
						value = default(T);
						return false;
					}
				}
			}
			catch (Exception e)
			{
				Log.Debug($"[{key}] failed conversion operation.");
				Log.Error(e);
				value = default(T);
				return false;
			}

			Log.Debug($"[{key}] is not in the storage.");
			value = default(T);
			return false;
		}

		/// <inheritdoc />
		public void UpdateValue<T>(string key, T value) where T : class
		{
			if (_values.ContainsKey(key))
			{
				Log.Debug($"Updating value for [{key}].");
				_values[key] = value;
			}
			else
			{
				Log.Debug($"Inserting value for [{key}].");
				_values.Add(key, value);
			}
		}

		/// <inheritdoc />
		public void Save()
		{
			try
			{
				Log.Debug($"Saving {nameof(SettingsStorage)}.");
				ApplicationSettings.Default.SettingsStorage = JsonConvert.SerializeObject(_values);
				ApplicationSettings.Default.Save();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}