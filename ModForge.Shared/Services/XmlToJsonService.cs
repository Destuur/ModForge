using Microsoft.Extensions.Logging;
using ModForge.Shared.Adapter;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Data;
using ModForge.Shared.Models.ModItems;
using System.Diagnostics;

namespace ModForge.Shared.Services
{
	public class XmlToJsonService
	{
		#region Private Fields
		private readonly DataSource dataSource;
		private readonly JsonAdapter jsonAdapter;
		private readonly LocalizationService localizationService;
		private readonly List<IDataPoint> dataPoints = new();
		private Dictionary<string, Dictionary<string, string>> localizationCache;
		private readonly UserConfigurationService userConfigurationService;
		private readonly ILogger<XmlToJsonService> logger;
		#endregion

		public XmlToJsonService(
			DataSource dataSource,
			JsonAdapter jsonAdapter,
			LocalizationService localizationService,
			UserConfigurationService userConfigurationService,
			ILogger<XmlToJsonService> logger
			)
		{
			this.dataSource = dataSource;
			this.jsonAdapter = jsonAdapter;
			this.localizationService = localizationService;
			this.userConfigurationService = userConfigurationService;
			this.logger = logger;
		}

		#region Properties
		public IList<IModItem> Perks { get; private set; }
		public IList<IModItem> Buffs { get; private set; }
		public IList<BuffParam> BuffParams { get; private set; }
		#endregion

		#region Public Methods
		public async Task ConvertXmlToJsonAsync()
		{
			var watch = Stopwatch.StartNew();
			try
			{
				ReadModItemsFromXml();
				AssignLocalizations();
				WriteModItemsAsJson();
				watch.Stop();

				logger.LogInformation("Conversion from XML to JSON completed in {ElapsedMilliseconds} ms", watch.ElapsedMilliseconds);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error during XML to JSON conversion.");
			}
		}

		public bool TryReadJsonFiles()
		{
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var perkPath = Path.Combine(appDataPath, "ModForge", "perks.json");
			var buffPath = Path.Combine(appDataPath, "ModForge", "buffs.json");

			if (!File.Exists(perkPath))
			{
				logger.LogWarning("Perk JSON file does not exist: {PerkPath}", perkPath);
				return false;
			}

			if (!File.Exists(buffPath))
			{
				logger.LogWarning("Buff JSON file does not exist: {BuffPath}", buffPath);
				return false;
			}

			try
			{
				Perks = ReadPerkJsonFile(perkPath).ToList();
				Buffs = ReadBuffJsonFile(buffPath).ToList();
				logger.LogInformation("Successfully read perk and buff JSON files.");
				return true;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error reading JSON files.");
				return false;
			}
		}
		#endregion

		#region Private Methods
		private void WriteModItemsAsJson()
		{
			try
			{
				jsonAdapter.WriteModItemsAsJson(Perks);
				jsonAdapter.WriteModItemsAsJson(Buffs);
				logger.LogInformation("Successfully wrote Perks and Buffs to JSON.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to write ModItems as JSON.");
			}
		}

		private IEnumerable<IModItem> ReadModItemsFromJson(string filePath)
		{
			try
			{
				var items = jsonAdapter.ReadModItemsFromJson(filePath);
				logger.LogInformation("Successfully read ModItems from JSON: {FilePath}", filePath);
				return items;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to read ModItems from JSON: {FilePath}", filePath);
				return Enumerable.Empty<IModItem>();
			}
		}

		private IEnumerable<IModItem> ReadPerkJsonFile(string filePath)
		{
			return ReadModItemsFromJson(filePath);
		}

		private IEnumerable<IModItem> ReadBuffJsonFile(string filePath)
		{
			return ReadModItemsFromJson(filePath);
		}

		private void AssignPerkLocalizations()
		{
			try
			{
				foreach (var perk in Perks)
				{
					foreach (var language in LocalizationAdapter.LanguageMap.Values)
					{
						if (!localizationCache.TryGetValue(language, out var langDict))
						{
							logger.LogWarning("Localization cache missing language: {Language}", language);
							continue;
						}

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
				logger.LogInformation("Perk localizations assigned successfully.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error occurred while assigning perk localizations.");
			}
		}

		private void AssignBuffLocalizations()
		{
			try
			{
				foreach (var buff in Buffs)
				{
					foreach (var language in LocalizationAdapter.LanguageMap.Values)
					{
						if (!localizationCache.TryGetValue(language, out var langDict))
						{
							logger.LogWarning("Localization cache does not contain language: {Language}", language);
							continue;
						}

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
				logger.LogInformation("Buff localizations assigned successfully.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error occurred while assigning buff localizations.");
			}
		}

		private string? GetAttributeValue(IEnumerable<IAttribute> attributes, params string[] names)
		{
			foreach (var name in names)
			{
				var attr = attributes.FirstOrDefault(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase));
				if (attr != null)
				{
					return attr.Value?.ToString();
				}
			}
			return null;
		}

		private void GetDataPoints()
		{
			try
			{
				dataPoints.Clear();

				foreach (var type in ToolResources.Keys.Endpoints())
				{
					foreach (var item in type.Value)
					{
						var fullPath = Path.Combine(userConfigurationService.Current.GameDirectory, item.Value);
						var dataPoint = DataPointFactory.CreateDataPoint(fullPath, item.Key, type.Key);
						if (dataPoint != null)
						{
							dataPoints.Add(dataPoint);
						}
						else
						{
							logger.LogWarning("Failed to create data point for path: {Path}, key: {Key}, type: {Type}", fullPath, item.Key, type.Key);
						}
					}
				}
				logger.LogInformation("Data points collected: {Count}", dataPoints.Count);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to get data points.");
			}
		}

		private void ReadModItemsFromXml()
		{
			try
			{
				GetDataPoints();

				Perks = ImportModItemsOfType(typeof(Perk));
				Buffs = ImportModItemsOfType(typeof(Buff));

				localizationCache = localizationService.ReadLocalizationFromXml(userConfigurationService.Current.GameDirectory);

				logger.LogInformation("Mod items and localization loaded successfully.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to read mod items or localization from XML.");
				Perks = new List<IModItem>();
				Buffs = new List<IModItem>();
				localizationCache = new Dictionary<string, Dictionary<string, string>>();
			}
		}

		private IList<IModItem> ImportModItemsOfType(Type type)
		{
			var foundList = new List<IModItem>();

			foreach (var dataPoint in dataPoints.Where(dp => dp.Type == type))
			{
				var items = dataSource.ReadModItems(dataPoint);
				if (items != null)
				{
					foundList.AddRange(items);
				}
				else
				{
					logger.LogWarning("No mod items found for data point: {DataPoint}", dataPoint);
				}
			}

			return foundList;
		}

		private void AssignLocalizations()
		{
			try
			{
				AssignPerkLocalizations();
				AssignBuffLocalizations();
				logger.LogInformation("Localizations assigned successfully.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error while assigning localizations.");
			}
		}
		#endregion
	}
}
