using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Adapter;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.Abstractions;
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
		private readonly List<Type> weaponTypes = ToolResources.Keys.GetWeaponTypes();
		private readonly List<Type> armorTypes = ToolResources.Keys.GetArmorTypes();
		private readonly List<Type> consumableTypes = ToolResources.Keys.GetConsumableTypes();
		private readonly List<Type> craftingMaterialTypes = ToolResources.Keys.GetCraftingMaterialsTypes();
		private readonly List<Type> miscTypes = ToolResources.Keys.GetMiscTypes();
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
		public IList<IModItem> Weapons { get; private set; } = new List<IModItem>();
		public IList<IModItem> Armors { get; private set; } = new List<IModItem>();
		public IList<IModItem> Consumeables { get; private set; } = new List<IModItem>();
		public IList<IModItem> CraftingMaterials { get; private set; } = new List<IModItem>();
		public IList<IModItem> MiscItems { get; private set; } = new List<IModItem>();
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

		public IModItem? GetModItem(string id)
		{
			return Perks.FirstOrDefault(x => x.Id == id) ??
				Buffs.FirstOrDefault(x => x.Id == id) ??
				Weapons.FirstOrDefault(x => x.Id == id) ??
				Armors.FirstOrDefault(x => x.Id == id) ??
				Consumeables.FirstOrDefault(x => x.Id == id) ??
				CraftingMaterials.FirstOrDefault(x => x.Id == id) ??
				MiscItems.FirstOrDefault(x => x.Id == id) ?? null;
		}
		#endregion

		#region Private Methods
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

				foreach (var type in weaponTypes)
				{
					Weapons = Weapons.Concat(ImportModItemsOfType(type)).ToList();
				}

				foreach (var type in armorTypes)
				{
					Armors = Armors.Concat(ImportModItemsOfType(type)).ToList();
				}

				foreach (var type in consumableTypes)
				{
					Consumeables = Consumeables.Concat(ImportModItemsOfType(type)).ToList();
				}

				foreach (var type in craftingMaterialTypes)
				{
					CraftingMaterials = CraftingMaterials.Concat(ImportModItemsOfType(type)).ToList();
				}

				foreach (var type in miscTypes)
				{
					MiscItems = MiscItems.Concat(ImportModItemsOfType(type)).ToList();
				}

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
