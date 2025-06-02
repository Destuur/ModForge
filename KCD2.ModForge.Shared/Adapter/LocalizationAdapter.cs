using KCD2.ModForge.Shared.Factories;
using KCD2.ModForge.Shared.Services;
using System.IO.Compression;
using System.Xml.Linq;

namespace KCD2.ModForge.Shared.Adapter
{
	public class LocalizationAdapter
	{
		private HashSet<string> relevantFiles = new HashSet<string>
		{
			"text_ui_soul.xml",
			//"text_ui_tutorials.xml",
			//"text_ui_quest.xml",
			//"text_ui_misc.xml",
			//"text_ui_minigames.xml",
			"text_ui_menus.xml",
			"text_ui_items.xml",
			//"text_ui_ingame.xml",
			//"text_ui_dialog.xml"
		};
		private readonly UserConfigurationService userConfigurationService;

		public LocalizationAdapter(UserConfigurationService userConfigurationService)
		{
			this.userConfigurationService = userConfigurationService;
		}

		public static readonly Dictionary<string, string> LanguageMap = new()
		{
			{ "English", "en" },
			{ "German", "de" },
			{ "French", "fr" },
			{ "Spanish", "es" },
			{ "Italian", "it" },
			{ "Czech", "cs" },
			{ "Polish", "pl" },
			{ "Russian", "ru" },
			{ "Chineses", "zs" },
			{ "Chineset", "zh" },
			{ "Korean", "ko" },
			{ "Japanese", "ja" },
			{ "Turkish", "tr" },
			{ "Portuguese", "pt" },
			{ "Ukrainian", "uk" }
		};

		public Dictionary<string, Dictionary<string, string>> LoadAllLocalizationsFromPaks()
		{
			var pakPaths = PathFactory.CreatePakPaths(userConfigurationService.Current.GameDirectory);
			var result = new Dictionary<string, Dictionary<string, string>>();

			foreach (var pakPath in pakPaths)
			{
				var fileName = Path.GetFileNameWithoutExtension(pakPath); // z. B. "German_xml"
				var langKey = LanguageMap.FirstOrDefault(pair => fileName.StartsWith(pair.Key)).Value;

				if (string.IsNullOrEmpty(langKey)) continue;

				using var archive = ZipFile.OpenRead(pakPath);

				foreach (var entry in archive.Entries)
				{
					var name = Path.GetFileName(entry.FullName).ToLowerInvariant();
					if (!relevantFiles.Contains(name)) continue;

					using var stream = entry.Open();
					var parsed = ParseLocalizationXml(stream); // siehe unten

					if (!result.ContainsKey(langKey))
						result[langKey] = new Dictionary<string, string>();

					foreach (var (key, value) in parsed)
					{
						result[langKey][key] = value; // ggf. überschreiben
					}
				}
			}

			return result;
		}

		public Dictionary<string, string> ParseLocalizationXml(Stream stream)
		{
			var result = new Dictionary<string, string>();
			var doc = XDocument.Load(stream);

			foreach (var row in doc.Descendants("Row"))
			{
				var cells = row.Elements("Cell").ToList();
				if (cells.Count < 2) continue;

				var key = cells[0].Value.Trim();
				var value = cells[2].Value.Trim();

				if (!string.IsNullOrWhiteSpace(key))
					result[key] = value;
			}

			return result;
		}

	}
}
