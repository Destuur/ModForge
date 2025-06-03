using KCD2.ModForge.Shared.Factories;
using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.Localizations;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using System.IO.Compression;
using System.Xml.Linq;

namespace KCD2.ModForge.Shared.Adapter
{
	public partial class XmlAdapterOfT<T> : IModItemAdapter<T>
		where T : IModItem
	{
		private readonly UserConfigurationService userConfigurationService;

		public XmlAdapterOfT(UserConfigurationService userConfigurationService)
		{
			this.userConfigurationService = userConfigurationService;
		}

		public Task Initialize()
		{
			return Task.CompletedTask;
		}

		public Task Deinitialize()
		{
			return Task.CompletedTask;
		}

		public async Task<IList<T>> ReadModItems(string path)
		{
			await Task.Yield();
			var filePath = PathFactory.CreateTablesPath(userConfigurationService.Current!.GameDirectory);
			var modItemPath = string.Empty;
			var foundModItems = new List<T>();

			if (typeof(Perk).IsAssignableFrom(typeof(T)))
			{
				modItemPath = GetPerks(filePath, modItemPath, foundModItems);
				GetPerkToBuff(filePath, foundModItems);
			}

			if (typeof(Buff).IsAssignableFrom(typeof(T)))
			{
				modItemPath = GetBuffs(filePath, modItemPath, foundModItems);
			}

			return foundModItems;
		}

		private void GetPerkToBuff(string filePath, List<T> foundModItems)
		{
			using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var entry in archive.Entries)
				{
					if (entry.FullName.EndsWith(".tbl"))
					{
						continue;
					}

					if (entry.FullName.Contains("perk_buff"))
					{
						using (Stream stream = entry.Open())
						{
							try
							{
								XDocument doc = XDocument.Load(stream);

								foreach (var perkElement in doc.Descendants("perk_buff"))
								{
									var buffId = perkElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "buff_id").Value;
									var perkId = perkElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "perk_id").Value;
									var perkItem = foundModItems.FirstOrDefault(x => x.Id == perkId);
									// TODO: Als eigene Property oder als Attribut hinzufügen?
									if (perkItem is Perk perk)
									{
										perk.BuffId = buffId;
										perk.Attributes.Add(new Attribute<string>("buff_id", buffId));
									}
								}
							}
							catch (Exception ex)
							{
								Console.WriteLine($"Fehler beim Parsen von {entry.FullName}: {ex.Message}");
							}
						}
					}
				}
			}
		}

		private static string GetPerks(string filePath, string modItemPath, List<T> foundModItems)
		{
			using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var entry in archive.Entries)
				{
					if (entry.FullName.EndsWith(".tbl"))
					{
						continue;
					}

					if (entry.FullName.Contains("perk__combat") ||
						entry.FullName.Contains("perk__hardcore") ||
						entry.FullName.Contains("perk__kcd2"))
					{
						using (Stream stream = entry.Open())
						{
							try
							{
								modItemPath = Extensions.GetEntryPath(entry);

								XDocument doc = XDocument.Load(stream);

								foreach (var perkElement in doc.Descendants("perk"))
								{
									var modItem = ModItemFactory<T>.CreateModItem(perkElement, entry.FullName);

									foundModItems.Add(modItem);
								}
							}
							catch (Exception ex)
							{
								Console.WriteLine($"Fehler beim Parsen von {entry.FullName}: {ex.Message}");
							}
						}
					}
				}
			}

			return modItemPath;
		}

		private static string GetBuffs(string filePath, string modItemPath, List<T> foundModItems)
		{
			using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var entry in archive.Entries)
				{
					if (entry.FullName.EndsWith(".tbl"))
					{
						continue;
					}

					if (entry.FullName.Contains("buff.xml") ||
						entry.FullName.Contains("buff__alchemy") ||
						entry.FullName.Contains("buff__perk") ||
						entry.FullName.Contains("buff__perk_hardcore") ||
						entry.FullName.Contains("buff__perk_kcd1"))
					{
						using (Stream stream = entry.Open())
						{
							try
							{
								modItemPath = Extensions.GetEntryPath(entry);

								XDocument doc = XDocument.Load(stream);

								foreach (var perkElement in doc.Descendants("buff"))
								{
									var modItem = ModItemFactory<T>.CreateModItem(perkElement, entry.FullName);

									//TODO: Platzhalter - löschen
									//if (modItem.Attributes.Count >= 11)
									//{
									//	continue;
									//}

									foundModItems.Add(modItem);
								}
							}
							catch (Exception ex)
							{
								Console.WriteLine($"Fehler beim Parsen von {entry.FullName}: {ex.Message}");
							}
						}
					}
				}
			}

			return modItemPath;
		}


		public Task<IList<T>> ReadAsync(string path)
		{
			throw new NotImplementedException();
		}

		Task<T> IModItemAdapter<T>.GetModItem(string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WriteElement(T modItem)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WriteElements(IEnumerable<T> modItem)
		{
			throw new NotImplementedException();
		}

		public bool WriteModItems(ModDescription mod)
		{
			WriteModManifest(mod);
			CreateLocalization(mod);
			AppendLocalization(mod);

			foreach (var modItem in mod.ModItems)
			{
				if (modItem is Perk perk)
				{
					WritePerk(mod.ModId, perk);
				}

				if (modItem is Buff buff)
				{
					WriteBuff(mod.ModId, buff);
				}
			}
			var pakPath = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", mod.ModId, "Data");
			CreateModPak(pakPath, Path.Combine(pakPath, mod.ModId + ".pak"));
			return true;
		}

		private void CreatePak(string sourceFolder, string localizationPakFile)
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

		private void CreateModPak(string baseFolder, string pakFileName)
		{
			if (File.Exists(pakFileName))
				File.Delete(pakFileName);

			// Pfad zum Zielordner sicherstellen (falls notwendig)
			var outputFolder = Path.GetDirectoryName(pakFileName);
			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);

			using (FileStream fs = new FileStream(pakFileName, FileMode.CreateNew))
			using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
			{
				var files = Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories);

				foreach (var file in files)
				{
					// Wenn die Datei die PAK-Datei selbst ist, überspringen
					if (Path.GetFullPath(file) == Path.GetFullPath(pakFileName))
						continue;

					string entryName = Path.GetRelativePath(baseFolder, file).Replace('\\', '/');

					var entry = archive.CreateEntry(entryName, CompressionLevel.NoCompression);

					using (var entryStream = entry.Open())
					using (var fileStream = File.OpenRead(file))
					{
						fileStream.CopyTo(entryStream);
					}
				}
			}
		}

		private bool WriteBuff(string modId, Buff buff)
		{
			string directoryUpToRpg = buff.Path.Substring(0, buff.Path.IndexOf("rpg") + "rpg".Length);
			var buffDirectory = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", modId, "Data", directoryUpToRpg);
			var buffFile = Path.Combine(buffDirectory, "buff__" + modId + ".xml");

			Directory.CreateDirectory(buffDirectory);

			if (File.Exists(buffFile))
				File.Delete(buffFile);

			var attributes = new List<XAttribute>();

			foreach (var kv in buff.Attributes)
			{
				if (kv.Name == "buff_params" && kv.Value is List<BuffParam> list)
				{
					string serialized = BuffParamSerializer.ToAttributeString(list);
					attributes.Add(new XAttribute(kv.Name, serialized));
				}
				else if (kv.Value is Enum enumValue)
				{
					attributes.Add(new XAttribute(kv.Name, Convert.ToInt32(enumValue)));
				}
				else if (kv.Value is bool boolValue)
				{
					attributes.Add(new XAttribute(kv.Name, boolValue.ToString().ToLower()));
				}
				else
				{
					attributes.Add(new XAttribute(kv.Name, kv.Value?.ToString() ?? string.Empty));
				}
			}

			var buffElement = new XElement("buff", attributes);

			var doc = new XDocument(
				new XDeclaration("1.0", "us-ascii", null),
				new XElement("database",
					new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
					new XAttribute("name", "barbora"),
					new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../database.xsd"),
					new XElement("buffs",
						new XAttribute("version", "1"),
						buffElement
					)
				)
			);

			doc.Save(buffFile);
			return true;
		}

		private bool WritePerk(string modId, Perk perk)
		{
			string directoryUpToRpg = perk.Path.Substring(0, perk.Path.IndexOf("rpg") + "rpg".Length);
			var perkDirectory = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", modId, "Data", directoryUpToRpg);
			var perkFile = Path.Combine(perkDirectory, "perk__" + modId + ".xml");

			// Ordner erstellen
			Directory.CreateDirectory(perkDirectory);

			// Wenn Datei existiert, löschen
			if (File.Exists(perkFile))
				File.Delete(perkFile);

			var attributes = perk.Attributes.Select(kv =>
			{
				if (kv.Value is Enum enumValue)
				{
					return new XAttribute(kv.Name, Convert.ToInt32(enumValue));
				}
				if (kv.Value is bool boolValue)
				{
					return new XAttribute(kv.Name, boolValue.ToString().ToLower());
				}
				else
				{
					return new XAttribute(kv.Name, kv.Value?.ToString() ?? string.Empty);
				}
			});

			// Perk-Element erzeugen
			var perkElement = new XElement("perk", attributes);

			// Ganze XML-Struktur aufbauen
			var doc = new XDocument(
				new XDeclaration("1.0", "us-ascii", null),
				new XElement("database",
					new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
					new XAttribute("name", "barbora"),
					new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../database.xsd"),
					new XElement("perks",
						new XAttribute("version", "1"),
						perkElement
					)
				)
			);

			// XML-Datei speichern
			doc.Save(perkFile);

			return true;
		}

		private bool CreateLocalization(ModDescription mod)
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
					var language = LocalizationAdapter.LanguageMap.FirstOrDefault(x => x.Value == languageKey).Key;
					localizationPath = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", mod.ModId, "Localization", language + "_xml", "text__" + mod.ModId + ".xml");

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

		private bool AppendLocalization(ModDescription mod)
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
					var language = LocalizationAdapter.LanguageMap.FirstOrDefault(x => x.Value == languageKey).Key;
					if (language == null)
						continue;

					var localizationFolder = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", mod.ModId, "Localization", language + "_xml");
					var localizationPath = Path.Combine(localizationFolder, "text__" + mod.ModId + ".xml");

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
				CreatePak(folder, pakPath);
			}

			return true;
		}

		private XElement CreateRow(string id, string value)
		{
			return new XElement("Row",
				new XElement("Cell", id),
				new XElement("Cell", ""), // TODO: Default Wert hinzufügen.
				new XElement("Cell", value?.Replace(" ", "&nbsp;")) // z. B. Encoding vorbereiten
			);
		}

		public bool WriteModManifest(ModDescription mod)
		{
			if (mod is null)
				return false;

			var path = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", mod.ModId);

			// Verzeichnisse anlegen
			Directory.CreateDirectory(Path.Combine(path, "Data"));
			Directory.CreateDirectory(Path.Combine(path, "Localization"));

			var manifestPath = Path.Combine(path, "mod.manifest");

			if (File.Exists(manifestPath))
				return true;

			// XML-Struktur aufbauen
			var doc = new XDocument(
				new XDeclaration("1.0", "utf-8", null),
				new XElement("kcd_mod",
					new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
					new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
					new XElement("info",
						new XElement("name", mod.Name),
						new XElement("description", mod.Description),
						new XElement("author", mod.Author),
						new XElement("version", mod.ModVersion),
						new XElement("created_on", mod.CreatedOn),
						new XElement("modid", mod.ModId),
						new XElement("modifies_level", mod.ModifiesLevel.ToString().ToLower())
					),
					new XElement("supports",
						mod.SupportsGameVersions.Select(v => new XElement("kcd_version", v))
					)
				)
			);

			doc.Save(manifestPath);
			return true;
		}

	}
}
