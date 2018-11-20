using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Company.Desktop.Framework.Extensions;
using Company.Desktop.Framework.Mvvm.Abstraction.Integration.ViewMapping;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Integration.ViewMapping
{
	public class DefaultRegexTemplateMapper : IDataTemplateMapper
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(DefaultRegexTemplateMapper));

		public IRegexDataTemplatePatternProvider[] PatternProviders { get; }

		public DefaultRegexTemplateMapper(IEnumerable<IRegexDataTemplatePatternProvider> patternProviders)
		{
			PatternProviders = patternProviders?.ToArray() ?? throw new ArgumentNullException(nameof(patternProviders));
		}

		/// <inheritdoc />
		public IEnumerable<(Type viewModelType, Type viewType)> GetMappings(IEnumerable<Type> viewModelTypes, IEnumerable<Type> viewTypes)
		{
			var vt = viewTypes.ToArray();
			var vmt = viewModelTypes.ToArray();
			var vtRegexes = PatternProviders.Select(d => d.ViewTypeRegex).ToArray();
			var vmtRegexes = PatternProviders.Select(d => d.ViewModelTypeRegex).ToArray();
			var viewModelLookup = vmt.SelectMany(d => GetPatternValues(d, vmtRegexes)).ToLookup(d => d.typeKey);
			var viewLookup = vt.SelectMany(d => GetPatternValues(d, vtRegexes)).ToLookup(d => d.typeKey);

			foreach (var mergedResult in GetMergedResults(viewLookup, viewModelLookup))
			{
				yield return (mergedResult.viewModelType, mergedResult.viewType);
			}
		}

		private IEnumerable<(Type viewModelType, Type viewType)> GetMergedResults(
			ILookup<string, (string typeKey, Type type)> viewLookup, 
			ILookup<string, (string typeKey, Type type)> viewModelLookup)
		{
			foreach (var viewGroup in viewLookup)
			{
				var hasView = false;
				foreach (var viewTuple in viewGroup)
				{
					var hasViewModel = false;

					foreach (var viewModelTuple in viewModelLookup[viewGroup.Key])
					{
						if (hasView)
						{
							Log.Error($"Ambigious View mapping [{viewModelTuple.type.FullName}] -> [{string.Join(", ", viewLookup[viewGroup.Key].Select(d => d.type))}]");
							continue;
						}

						if (hasViewModel)
						{
							Log.Error($"Ambigious ViewModel mapping [{viewTuple.type.FullName}] -> [{string.Join(", ", viewModelLookup[viewGroup.Key].Select(d => d.type))}]");
							continue;
						}

						Log.Debug($"ViewModel [{viewModelTuple.type.FullName}] is rendered using [{viewTuple.type.FullName}]");
						yield return (viewModelTuple.type, viewTuple.type);

						hasViewModel = true;
					}

					hasView = true;
				}
			}
		}

		private IEnumerable<(string typeKey, Type type)> GetPatternValues(Type type, Regex[] patterns)
		{
			foreach (var pattern in patterns)
			{
				if(!pattern.TryMatch(type.FullName, out var match))
					continue;

				if (!match.Groups["ns1"].Success)
				{
					Log.Error($"{nameof(DefaultRegexTemplateMapper)} requires patterns provided by [{nameof(IRegexDataTemplatePatternProvider)}] to declare the named group [ns1]");
					continue;
				}
				if (!match.Groups["ns2"].Success)
				{
					Log.Error($"{nameof(DefaultRegexTemplateMapper)} requires patterns provided by [{nameof(IRegexDataTemplatePatternProvider)}] to declare the named group [ns2]");
					continue;
				}
				if (!match.Groups["class"].Success)
				{
					Log.Error($"{nameof(DefaultRegexTemplateMapper)} requires patterns provided by [{nameof(IRegexDataTemplatePatternProvider)}] to declare the named group [class]");
					continue;
				}

				var sb = new StringBuilder();
				sb.Append(match.Groups["ns1"].Value);
				sb.Append(match.Groups["ns2"].Value);
				sb.Append(match.Groups["class"].Value);

				yield return (sb.ToString(), type);
			}
		}
	}
}