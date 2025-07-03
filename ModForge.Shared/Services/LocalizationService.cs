using Microsoft.Extensions.Logging;
using ModForge.Shared.Adapter;
using ModForge.Shared.Models.Localizations;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Models.User;

namespace ModForge.Shared.Services
{
	public class LocalizationService
	{
		private readonly LocalizationAdapter adapter;
		private readonly ILogger<LocalizationService> logger;
		private Dictionary<string, Dictionary<string, string>> localizations;
		private UserConfigurationService userConfigurationService;

		public LocalizationService(LocalizationAdapter adapter, ILogger<LocalizationService> logger, UserConfigurationService userConfigurationService)
		{
			this.adapter = adapter;
			this.logger = logger;
			this.userConfigurationService = userConfigurationService;

			InitializeLocalizations(userConfigurationService.Current);
		}

		public void InitializeLocalizations(UserConfiguration userConfiguration)
		{
			if (userConfiguration is not null)
			{
				localizations = ReadLocalizationFromXml(userConfiguration.GameDirectory);
			}
		}

		public string? GetName(IModItem modItem)
		{
			try
			{
				var lang = userConfigurationService.Current.Language;
				var attribute = modItem.Attributes.FirstOrDefault(x => x.Name.Contains("ui_name")) ?? modItem.Attributes.FirstOrDefault(x => x.Name.Contains("UIName"));


				if (attribute is null)
				{
					var name = modItem.Attributes.FirstOrDefault(x => x.Name.ToLower().Contains("name")).Value;
					return name.ToString();
				}

				var key = attribute.Value.ToString();
				return localizations.TryGetValue(lang, out var value) ? value[key] : null;
			}
			catch (Exception e)
			{
				return "Test";
			}
		}

		public string? GetLoreDescription(IModItem modItem)
		{
			try
			{
				var lang = userConfigurationService.Current.Language;
				var attribute = modItem.Attributes.FirstOrDefault(x => x.Name.Contains("ui_lore_desc"));

				if (attribute is null)
				{
					return "No lore description found";
				}

				var key = attribute.Value.ToString();
				return localizations.TryGetValue(lang, out var value) ? value[key] : null;
			}
			catch (Exception e)
			{
				return "Test";
			}
		}

		public string? GetDescription(IModItem modItem)
		{
			try
			{
				var lang = userConfigurationService.Current.Language;
				var attribute = modItem.Attributes.FirstOrDefault(x => x.Name.Contains("ui_desc"));

				if (attribute is null)
				{
					return "No description found";
				}

				var key = attribute.Value.ToString();
				return localizations.TryGetValue(lang, out var value) ? value[key] : null;
			}
			catch (Exception e)
			{
				return "Test";
			}
		}


		public Dictionary<string, Dictionary<string, string>> ReadLocalizationFromXml(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				logger.LogWarning("ReadLocalizationFromXml was called with null or empty path.");
				return new Dictionary<string, Dictionary<string, string>>();
			}

			try
			{
				return adapter.ReadLocalizationFromXml(path);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to read localization from XML at path: {Path}", path);
				return new Dictionary<string, Dictionary<string, string>>();
			}
		}

		public void WriteLocalizationAsXml(string path, ModDescription mod)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				logger.LogWarning("WriteLocalizationAsXml called with null or empty path.");
				return;
			}

			if (mod == null)
			{
				logger.LogWarning("WriteLocalizationAsXml called with null ModDescription.");
				return;
			}

			try
			{
				adapter.WriteLocalizationAsXml(path, mod);
				logger.LogInformation("Localization successfully written to XML at path: {Path}", path);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to write localization XML to path: {Path}", path);
			}
		}
	}
}
