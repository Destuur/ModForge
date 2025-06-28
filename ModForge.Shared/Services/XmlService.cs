using Microsoft.Extensions.Logging;
using ModForge.Shared.Adapter;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Data;
using ModForge.Shared.Models.Localizations;
using ModForge.Shared.Models.ModItems;
using System.Diagnostics;

namespace ModForge.Shared.Services
{
	public class XmlService
	{
		#region Private Fields
		private readonly DataSource dataSource;
		private readonly JsonAdapter jsonAdapter;
		private readonly LocalizationService localizationService;
		private readonly List<IDataPoint> dataPoints = new();
		private Dictionary<string, Dictionary<string, string>> localizationCache;
		private readonly UserConfigurationService userConfigurationService;
		private readonly ILogger<XmlService> logger;
		#endregion

		public XmlService(
			DataSource dataSource,
			JsonAdapter jsonAdapter,
			LocalizationService localizationService,
			UserConfigurationService userConfigurationService,
			ILogger<XmlService> logger
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
		public void ConvertXml()
		{
			var watch = Stopwatch.StartNew();
			try
			{
				ReadModItemsFromXml();
				watch.Stop();

				logger.LogInformation("Conversion from XML to JSON completed in {ElapsedMilliseconds} ms", watch.ElapsedMilliseconds);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error during XML to JSON conversion.");
			}
		}

		public bool TryReadJsonFilesWithFallback()
		{
			if (string.IsNullOrEmpty(userConfigurationService.Current.GameDirectory))
			{
				return false;
			}

			if (string.IsNullOrEmpty(userConfigurationService.Current.GameDirectory) == false)
			{
				ConvertXml();
			}

			logger.LogError("Failed to recover JSON files after fallback conversion.");
			return false;
		}
		#endregion

		#region Private Methods

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
		#endregion
	}
}
