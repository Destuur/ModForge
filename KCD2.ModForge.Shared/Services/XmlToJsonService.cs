using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.Data;
using KCD2.ModForge.Shared.Models.ModItems;
using System.Diagnostics;

namespace KCD2.ModForge.Shared.Services
{
	public class XmlToJsonService
	{
		private readonly DataSource dataSource;
		private readonly JsonAdapter jsonAdapter;
		private readonly LocalizationService localizationService;
		private readonly List<IDataPoint> dataPoints;
		private Dictionary<string, Dictionary<string, string>> localizationCache;
		private readonly UserConfigurationService userConfigurationService;

		public XmlToJsonService(DataSource dataSource, List<IDataPoint> dataPoints, JsonAdapter jsonAdapter, LocalizationService localizationService, UserConfigurationService userConfigurationService)
		{
			this.dataSource = dataSource;
			this.jsonAdapter = jsonAdapter;
			this.localizationService = localizationService;
			this.dataPoints = dataPoints.ToList();
			this.userConfigurationService = userConfigurationService;
		}

		public IList<IModItem> Perks { get; private set; }
		public IList<IModItem> Buffs { get; private set; }
		public IList<BuffParam> BuffParams { get; private set; }

		private void ReadModItemsFromXml()
		{
			Perks = ImportPerksFromXml();
			Buffs = ImportBuffsFromXml();
			localizationCache = localizationService.ReadLocalizationFromXml(userConfigurationService.Current.GameDirectory);
		}

		private IList<IModItem> ImportPerksFromXml()
		{
			var foundList = new List<IModItem>();

			foreach (var dataPoint in dataPoints)
			{
				if (dataPoint.Type == typeof(Perk))
				{
					foundList = foundList.Concat(dataSource.ReadModItems(dataPoint)).ToList();
				}
			}

			return foundList;
		}

		private IList<IModItem> ImportBuffsFromXml()
		{
			var foundList = new List<IModItem>();

			foreach (var dataPoint in dataPoints)
			{
				if (dataPoint.Type == typeof(Buff))
				{
					foundList = foundList.Concat(dataSource.ReadModItems(dataPoint)).ToList();
				}
			}

			return foundList;
		}

		private void AssignLocalizations()
		{
			AssignPerkLocalizations();
			AssignBuffLocalizations();
		}

		public void ConvertXmlToJsonAsync()
		{
			var watch = Stopwatch.StartNew();
			ReadModItemsFromXml();
			AssignLocalizations();
			WriteModItemsAsJson();
			watch.Stop();
		}

		private void WriteModItemsAsJson()
		{
			jsonAdapter.WriteModItemsAsJson(Perks);
			jsonAdapter.WriteModItemsAsJson(Buffs);
		}

		public bool TryReadJsonFiles()
		{
			var perkPath = Path.Combine(
							Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
							"ModForge", $"perks.json");
			var buffPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"ModForge", $"buffs.json");

			if (File.Exists(perkPath) == false)
			{
				return false;
			}

			if (File.Exists(buffPath) == false)
			{
				return false;
			}

			Perks = ReadPerkJsonFile(perkPath).ToList();
			Buffs = ReadBuffJsonFile(buffPath).ToList();
			return true;
		}

		private IEnumerable<IModItem> ReadPerkJsonFile(string filePath)
		{
			return jsonAdapter.ReadModItemsFromJson(filePath);
		}

		private IEnumerable<IModItem> ReadBuffJsonFile(string filePath)
		{
			return jsonAdapter.ReadModItemsFromJson(filePath);
		}

		private void AssignPerkLocalizations()
		{

			foreach (var perk in Perks)
			{
				foreach (var language in LocalizationAdapter.LanguageMap.Values)
				{
					if (localizationCache.TryGetValue(language, out var langDict))
					{
						var descKey = GetAttributeValue(perk.Attributes, "perk_ui_desc");
						var loreDescKey = GetAttributeValue(perk.Attributes, "perk_ui_lore_desc");
						var nameKey = GetAttributeValue(perk.Attributes, "perk_ui_name");

						if (descKey != null && langDict.TryGetValue(descKey, out var desc))
						{
							perk.Localization.Descriptions[language] = new Dictionary<string, string> { [descKey] = desc };
						}

						if (loreDescKey != null && langDict.TryGetValue(loreDescKey, out var loreDesc))
						{
							perk.Localization.LoreDescriptions[language] = new Dictionary<string, string> { [loreDescKey] = loreDesc };
						}

						if (nameKey != null && langDict.TryGetValue(nameKey, out var name))
						{
							perk.Localization.Names[language] = new Dictionary<string, string> { [nameKey] = name };
						}
					}
				}
			}
		}

		private void AssignBuffLocalizations()
		{
			foreach (var buff in Buffs)
			{
				foreach (var language in LocalizationAdapter.LanguageMap.Values)
				{
					if (localizationCache.TryGetValue(language, out var langDict))
					{
						var descKey = GetAttributeValue(buff.Attributes, "buff_desc");
						var loreDescKey = GetAttributeValue(buff.Attributes, "slot_buff_ui_name");
						var uiNameKey = GetAttributeValue(buff.Attributes, "buff_ui_name");

						if (descKey != null && langDict.TryGetValue(descKey, out var desc))
						{
							buff.Localization.Descriptions[language] = new Dictionary<string, string> { [descKey] = desc };
						}

						if (loreDescKey != null && langDict.TryGetValue(loreDescKey, out var loreDesc))
						{
							buff.Localization.LoreDescriptions[language] = new Dictionary<string, string> { [loreDescKey] = loreDesc };
						}

						if (uiNameKey != null && langDict.TryGetValue(uiNameKey, out var name))
						{
							buff.Localization.Names[language] = new Dictionary<string, string> { [uiNameKey] = name };
						}
					}
				}
			}
		}

		private string? GetAttributeValue(IEnumerable<IAttribute> attributes, params string[] names)
		{
			foreach (var name in names)
			{
				var attr = attributes.FirstOrDefault(a => a.Name == name);
				if (attr != null)
				{
					return attr.Value?.ToString();
				}
			}
			return null;
		}
	}
}
