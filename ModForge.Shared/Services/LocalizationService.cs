using Microsoft.Extensions.Logging;
using ModForge.Shared.Adapter;
using ModForge.Shared.Models.Localizations;
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

		private void InitializeLocalizations(UserConfiguration userConfiguration)
		{
			if (userConfiguration is not null)
			{
				localizations = ReadLocalizationFromXml(userConfiguration.GameDirectory);
			}
		}

		public string? GetName(string language, string key)
		{
			return localizations.TryGetValue(language, out var value) ? value[key] : null;
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
