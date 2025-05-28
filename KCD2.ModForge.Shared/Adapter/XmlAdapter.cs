using KCD2.ModForge.Shared.Models;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Mods;
using KCD2.ModForge.Shared.Services;
using System.IO.Compression;
using System.Xml.Linq;


namespace KCD2.ModForge.Shared.Adapter
{
	public class XmlAdapter : IXmlAdapter
	{
		private string tablePath => ToolResources.Keys.TablesPath();
		private string localizationPath => ToolResources.Keys.EnglishLocalizationPath();
		private string iconPath => ToolResources.Keys.IconPath();
		private readonly LocalizationService localizationService;
		private readonly IconService iconService;
		private readonly PerkService perkService;
		private List<IModItem> modItems = new();

		public XmlAdapter(LocalizationService localizationService, IconService iconService, PerkService perkService)
		{
			this.localizationService = localizationService;
			this.iconService = iconService;
			this.perkService = perkService;
		}

		public async Task Initialize()
		{
			await Task.Yield();


			InitializePerks();

			//TODO: Buffs und Localizations initializieren!
			//InitializeBuffs();
			//InitializeIcons();
			InitializeLocalizations();
		}

		private void InitializeLocalizations()
		{
			if (localizationService.IsFilled(Language.English))
			{
				return;
			}

			string path = string.Empty;

			using (FileStream zipToOpen = new FileStream(localizationPath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var archiveEntry in archive.Entries)
				{
					if (archiveEntry.FullName.Contains("text"))
					{
						using (Stream stream = archiveEntry.Open())
						{
							try
							{
								//path = Extensions.GetEntryPath(archiveEntry);

								XDocument doc = XDocument.Load(stream);

								var entries = doc.Root!.Elements("Row").Select(row =>
								{
									var cells = row.Elements("Cell").ToList();

									return new
									{
										Key = cells.ElementAtOrDefault(0)?.Value,
										Value = cells.ElementAtOrDefault(2)?.Value
									};
								})
								.Where(e => !string.IsNullOrEmpty(e.Key) && !string.IsNullOrEmpty(e.Value))
								.ToList();

								foreach (var entry in entries)
								{
									var localization = Localization.GetLocalization(entry.Key, entry.Value, archiveEntry.FullName);

									localizationService.AddLocalization(localization);
								}
							}
							catch (Exception ex)
							{
								Console.WriteLine($"Fehler beim Parsen von {archiveEntry.FullName}: {ex.Message}");
							}
						}
					}
				}
			}
		}

		private void InitializeBuffs()
		{
			throw new NotImplementedException();
		}

		private void InitializePerks()
		{
			if (modItems.Count != 0)
			{
				return;
			}
			string path = string.Empty;

			using (FileStream zipToOpen = new FileStream(tablePath, FileMode.Open))
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
								//path = Extensions.GetEntryPath(entry);

								XDocument doc = XDocument.Load(stream);

								foreach (var perkElement in doc.Descendants("perk"))
								{
									//var perk = Perk.GetPerk(perkElement, entry.FullName);

									////TODO: Platzhalter - löschen
									//if (perk.Attributes.Count >= 11)
									//{
									//	continue;
									//}

									//perkService.AddPerk(perk);
									////modItems.Add(perk);
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

		public async Task Deinitialize()
		{
			await Task.Yield();
			modItems.Clear();
		}

		public async Task<IModItem> GetModItem(string id)
		{
			await Task.Yield();

			if (string.IsNullOrEmpty(id))
			{
				return null!;
			}

			var modItem = modItems.FirstOrDefault(x => x.Id == id);

			if (modItem is null)
			{
				return null!;
			}

			return modItem;
		}

		public async Task<List<IModItem>> GetModItems()
		{
			await Task.Yield();
			return modItems;
		}

		public async Task<bool> WriteModItems(ModDescription mod)
		{
			await CreateLocalization(mod.ModId);

			foreach (var modItem in mod.ModItems)
			{
				if (modItem is Perk perk)
				{
					foreach (var localization in perk.Localizations)
					{
						await AppendLocalization(mod.ModId, localization, "text__");
					}

					await WritePerk(mod.ModId, perk, "perk__");
					continue;
				}
			}

			var sourceFolder = Path.Combine(ToolResources.Keys.ModPath() + "\\" + mod.ModId + "\\Localization\\German_xml");
			var localizationPakFile = Path.Combine(ToolResources.Keys.ModPath() + "\\" + mod.ModId + "\\Localization\\German_xml.pak");
			CreatePak(sourceFolder, localizationPakFile);
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

		private async Task WritePerk(string modId, Perk perk, string v)
		{
			Directory.CreateDirectory(ToolResources.Keys.ModPath() + modId + "\\Data\\" + perk.Path);

			File.Delete(Path.Combine(ToolResources.Keys.ModPath() + modId + "\\Data\\" + perk.Path, "perk__" + ToolResources.Keys.ModId() + ".xml"));

			using (StreamWriter writer = new StreamWriter(Path.Combine(ToolResources.Keys.ModPath() + modId + "\\Data\\" + perk.Path, "perk__" + ToolResources.Keys.ModId() + ".xml")))
			{
				writer.WriteLine($"<?xml version=\"1.0\" encoding=\"us-ascii\"?>");
				writer.WriteLine($"<database xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" name=\"barbora\" xsi:noNamespaceSchemaLocation=\"../database.xsd\">");
				writer.WriteLine($"    <perks version=\"1\">");
			}

			using (StreamWriter writer = File.AppendText(Path.Combine(ToolResources.Keys.ModPath() + modId + "\\Data\\" + perk.Path, "perk__" + ToolResources.Keys.ModId() + ".xml")))
			{
				// Dictionary in XAttribute-Liste umwandeln
				//var attributes = perk.Attributes.Select(kv => new XAttribute(kv.Key, kv.Value));

				//// Ein XElement mit diesen Attributen erstellen
				//var element = new XElement("perk", attributes);

				//// In eine Datei oder einen Stream schreiben
				//writer.WriteLine($"        {element}");
			}

			using (StreamWriter writer = File.AppendText(Path.Combine(ToolResources.Keys.ModPath() + modId + "\\Data\\" + perk.Path, "perk__" + ToolResources.Keys.ModId() + ".xml")))
			{
				writer.WriteLine($"    </perks>");
				writer.WriteLine($"</database>");
			}
		}

		// TODO: Sprache identifizieren und in den Pfad einbauen
		private async Task CreateLocalization(string modId)
		{
			var localizationPath = Path.Combine(ToolResources.Keys.ModPath() + "\\" + modId + "\\Localization\\German_xml\\text__" + modId + ".xml");

			var directory = Path.GetDirectoryName(localizationPath);
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			var doc = new XDocument(
				new XElement("Table"));
			doc.Save(localizationPath);
		}

		private async Task AppendLocalization(string modId, Localization localization, string v)
		{
			var localizationPath = Path.Combine(ToolResources.Keys.ModPath() + "\\" + modId + "\\Localization\\German_xml\\text__" + modId + ".xml");

			var doc = XDocument.Load(localizationPath);
			var root = doc.Element("Table");

			if (root != null)
			{
				var newRow = new XElement("Row",
					new XElement("Cell", localization.Id),
					new XElement("Cell", "Nonsense"),
					new XElement("Cell", localization.Value) // z. B. mit &amp;nbsp; vorbereiten
				);

				root.Add(newRow);
				doc.Save(localizationPath);
			}
		}

		public async Task<bool> WriteModManifest(ModDescription modDescription)
		{
			await Task.Yield();

			if (modDescription is null)
			{
				return false;
			}

			var path = ToolResources.Keys.ModPath() + $"\\{modDescription.ModId}";

			Directory.CreateDirectory(path + "\\Data");
			Directory.CreateDirectory(path + "\\Localization");

			if (File.Exists(Path.Combine(path, "mod.manifest")) == false)
			{
				using (StreamWriter writer = new StreamWriter(Path.Combine(path, "mod.manifest")))
				{
					writer.WriteLine($"<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine($"<kcd_mod xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
					writer.WriteLine($"	<info>");
					writer.WriteLine($"		<name>{modDescription.Name}</name>");
					writer.WriteLine($"		<description>{modDescription.Description}</description>");
					writer.WriteLine($"		<author>{modDescription.Author}</author>");
					writer.WriteLine($"		<version>{modDescription.ModVersion}</version>");
					writer.WriteLine($"		<created_on>{modDescription.CreatedOn}</created_on>");
					writer.WriteLine($"		<modid>{modDescription.ModId}</modid>");
					writer.WriteLine($"		<modifies_level>{modDescription.ModifiesLevel.ToString().ToLower()}</modifies_level>");
					writer.WriteLine($"	</info>");
					writer.WriteLine($"	<supports>");
					foreach (var version in modDescription.SupportsGameVersions)
					{
						writer.WriteLine($"		<kcd_version>{version}</kcd_version>");
					}
					writer.WriteLine($"	</supports>");
					writer.WriteLine($"</kcd_mod>");
				}
			}

			return true;
		}
	}
}
