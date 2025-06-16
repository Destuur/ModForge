using Microsoft.Extensions.Logging;
using ModForge.Shared.Adapter;
using ModForge.Shared.Models.Localizations;
using ModForge.Shared.Models.Mods;

namespace ModForge.Shared.Services
{
	public class LocalizationService
	{
		private readonly LocalizationAdapter adapter;
		private readonly ILogger<LocalizationService> logger;
		private List<Localization> enLocalizations = new();
		private List<Localization> deLocalizations = new();

		public LocalizationService(LocalizationAdapter adapter, ILogger<LocalizationService> logger)
		{
			this.adapter = adapter;
			this.logger = logger;
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
