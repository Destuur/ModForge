using ModForge.Shared.Factories;
using ModForge.Shared.Models.Localizations;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using System.IO.Compression;
using System.Xml.Linq;

namespace ModForge.Shared.Adapter
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

		public Dictionary<string, Dictionary<string, string>> ReadLocalizationFromXml(string path)
		{
			var pakPaths = PathFactory.CreatePakPaths(path);
			var result = new Dictionary<string, Dictionary<string, string>>();

			foreach (var pakPath in pakPaths)
			{
				var fileName = Path.GetFileNameWithoutExtension(pakPath); // z. B. "German_xml"
				var langKey = Languages.Map.FirstOrDefault(pair => fileName.StartsWith(pair.Key)).Value;

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

		public void WriteLocalizationAsXml(string path, ModDescription mod)
		{
			CreateLocalization(path, mod);
			AppendLocalization(path, mod);
		}

		private bool CreateLocalization(string path, ModDescription mod)
		{
			string localizationPath;
			Dictionary<string, Dictionary<string, string>> names;
			Dictionary<string, Dictionary<string, string>> descriptions;
			Dictionary<string, Dictionary<string, string>> loreDescriptions;

			foreach (var moditem in mod.ModItems)
			{
				if (moditem.Localization.Names.Count == 0 &&
					moditem.Localization.Descriptions.Count == 0 &&
					moditem.Localization.LoreDescriptions.Count == 0)
				{
					continue;
				}

				names = moditem.Localization.Names;
				descriptions = moditem.Localization.Descriptions;
				loreDescriptions = moditem.Localization.LoreDescriptions;

				var allLanguages = names.Keys
					.Union(descriptions.Keys)
					.Union(loreDescriptions.Keys)
					.Distinct();

				foreach (var languageKey in allLanguages)
				{
					var language = Languages.Map.FirstOrDefault(x => x.Value == languageKey).Key;
					localizationPath = PathFactory.CreateExportLocalizationPath(path, language, mod.Id);

					var directory = Path.GetDirectoryName(localizationPath);

					if (!Directory.Exists(directory))
					{
						Directory.CreateDirectory(directory);
					}

					var doc = new XDocument(new XElement("Table"));
					doc.Save(localizationPath);
				}
			}
			return true;
		}

		private bool AppendLocalization(string path, ModDescription mod)
		{
			var modifiedLocalizationFolders = new HashSet<string>();

			foreach (var modItem in mod.ModItems)
			{
				var loc = modItem.Localization;

				if (loc.Names.Count == 0 &&
					   loc.Descriptions.Count == 0 &&
					   loc.LoreDescriptions.Count == 0)
				{
					continue;
				}

				var allLanguages = loc.Names.Keys
					.Union(loc.Descriptions.Keys)
					.Union(loc.LoreDescriptions.Keys)
					.Distinct();

				foreach (var languageKey in allLanguages)
				{
					var language = Languages.Map.FirstOrDefault(x => x.Value == languageKey).Key;
					if (language == null)
						continue;

					var localizationFolder = Path.Combine(path, "Mods", mod.Id, "Localization", language + "_xml");
					var localizationPath = Path.Combine(localizationFolder, "text__" + mod.Id + ".xml");

					if (!File.Exists(localizationPath))
						continue;

					var doc = XDocument.Load(localizationPath);
					var root = doc.Element("Table");

					if (root == null)
						continue;

					if (loc.Names.TryGetValue(languageKey, out var nameValue))
						root.Add(CreateRow(nameValue.Keys.First(), nameValue.Values.First()));

					if (loc.Descriptions.TryGetValue(languageKey, out var descValue))
						root.Add(CreateRow(descValue.Keys.First(), descValue.Values.First()));

					if (loc.LoreDescriptions.TryGetValue(languageKey, out var loreValue))
						root.Add(CreateRow(loreValue.Keys.First(), loreValue.Values.First()));

					doc.Save(localizationPath);

					modifiedLocalizationFolders.Add(localizationFolder); // merken
				}
			}

			// Jetzt alle geänderten Ordner packen
			foreach (var folder in modifiedLocalizationFolders)
			{
				var pakPath = folder + ".pak";
				CreateLocalizationPak(folder, pakPath);
			}

			return true;
		}

		private void CreateLocalizationPak(string sourceFolder, string localizationPakFile)
		{
			if (File.Exists(localizationPakFile))
				File.Delete(localizationPakFile);

			using (FileStream fs = new FileStream(localizationPakFile, FileMode.CreateNew))
			using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
			{
				var files = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories);

				foreach (var file in files)
				{
					// Pfad innerhalb des Archives, relativ zum sourceDir
					string entryName = Path.GetRelativePath(sourceFolder, file).Replace('\\', '/');

					var entry = archive.CreateEntry(entryName, CompressionLevel.NoCompression);

					using (var entryStream = entry.Open())
					using (var fileStream = File.OpenRead(file))
					{
						fileStream.CopyTo(entryStream);
					}
				}
			}
		}

		private XElement CreateRow(string id, string value)
		{
			return new XElement("Row",
				new XElement("Cell", id),
				new XElement("Cell", ""), // TODO: Default Wert hinzufügen.
				new XElement("Cell", value?.Replace(" ", "&nbsp;")) // z. B. Encoding vorbereiten
			);
		}
	}
}
