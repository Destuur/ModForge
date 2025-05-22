using KCD2.XML.Tool.Shared.Models;
using KCD2.XML.Tool.Shared.Mods;
using KCD2.XML.Tool.Shared.Services;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace KCD2.XML.Tool.Shared.Adapter
{
	public class XmlAdapter : IXmlAdapter
	{
		private string tablesPath => ToolRessources.Keys.TablesPath();
		private string localizationsPath => ToolRessources.Keys.LocalizationPath();
		private readonly LocalizationService localizationService;
		private List<IModItem> modItems = new();

		public XmlAdapter(LocalizationService localizationService)
		{
			this.localizationService = localizationService;
		}

		public async Task Initialize()
		{
			await Task.Yield();


			InitializePerks();

			//TODO: Buffs und Localizations initializieren!
			//InitializeBuffs();
			InitializeLocalizations();
		}

		private void InitializeLocalizations()
		{
			string path = string.Empty;

			using (FileStream zipToOpen = new FileStream(localizationsPath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var archiveEntry in archive.Entries)
				{

					path = Extensions.GetEntryPath(archiveEntry);

					if (archiveEntry.FullName.Contains("text"))
					{
						using (Stream stream = archiveEntry.Open())
						{
							try
							{
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
									var localization = Localization.GetLocalization(entry.Key, entry.Value, path);

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
			string path = string.Empty;

			using (FileStream zipToOpen = new FileStream(tablesPath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var entry in archive.Entries)
				{

					path = Extensions.GetEntryPath(entry);

					if (entry.FullName.Contains("perk__combat") ||
						entry.FullName.Contains("perk__hardcore") ||
						entry.FullName.Contains("perk__kcd2"))
					{
						using (Stream stream = entry.Open())
						{
							try
							{
								XDocument doc = XDocument.Load(stream);

								foreach (var perkElement in doc.Descendants("perk"))
								{
									var perk = Perk.GetPerk(perkElement, path);

									modItems.Add(perk);
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

		public async Task<bool> WriteModItems(IEnumerable<IModItem> modItems)
		{
			await Task.Yield();
			Directory.CreateDirectory(ToolRessources.Keys.ModPath() + "\\Data");
			Directory.CreateDirectory(ToolRessources.Keys.ModPath() + "\\Localization");



			var path = ToolRessources.Keys.ModPath();

			if (File.Exists(Path.Combine(path, "mod.manifest")) == false)
			{
				using (StreamWriter writer = new StreamWriter(Path.Combine(path, "mod.manifest")))
				{
					writer.WriteLine($"<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine($"<kcd_mod xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
					writer.WriteLine($"	<info>");
					writer.WriteLine($"		<name>Test Mod</name>");
					writer.WriteLine($"		<description>Test Mod by XML Tool</description>");
					writer.WriteLine($"		<author>Destuur</author>");
					writer.WriteLine($"		<version>1.0</version>");
					writer.WriteLine($"		<created_on>{XmlConvert.ToString(DateTime.Now, XmlDateTimeSerializationMode.Utc)}</created_on>");
					writer.WriteLine($"		<modid>{ToolRessources.Keys.ModId()}</modid>");
					writer.WriteLine($"		<modifies_level>false</modifies_level>");
					writer.WriteLine($"	</info>");
					writer.WriteLine($"</kcd_mod>");
				}
			}

			foreach (var item in modItems)
			{
				Directory.CreateDirectory(ToolRessources.Keys.ModPath() + "\\Data\\" + item.Path);

				File.Delete(Path.Combine(ToolRessources.Keys.ModPath() + "\\Data\\" + item.Path, "perk__" + ToolRessources.Keys.ModId() + ".xml"));

				using (StreamWriter writer = new StreamWriter(Path.Combine(ToolRessources.Keys.ModPath() + "\\Data\\" + item.Path, "perk__" + ToolRessources.Keys.ModId() + ".xml")))
				{
					writer.WriteLine($"<?xml version=\"1.0\" encoding=\"us-ascii\"?>");
					writer.WriteLine($"<database xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" name=\"barbora\" xsi:noNamespaceSchemaLocation=\"../database.xsd\">");
					writer.WriteLine($"    <perks version=\"1\">");
				}
			}

			foreach (var item in modItems)
			{
				using (StreamWriter writer = File.AppendText(Path.Combine(ToolRessources.Keys.ModPath() + "\\Data\\" + item.Path, "perk__" + ToolRessources.Keys.ModId() + ".xml")))
				{
					if (item is Perk perk)
					{
						// Dictionary in XAttribute-Liste umwandeln
						var attributes = perk.Attributes.Select(kv => new XAttribute(kv.Key, kv.Value));

						// Ein XElement mit diesen Attributen erstellen
						var element = new XElement("perk", attributes);

						// In eine Datei oder einen Stream schreiben
						writer.WriteLine($"        {element}");
					}
				}
			}


			foreach (var item in modItems)
			{
				using (StreamWriter writer = File.AppendText(Path.Combine(ToolRessources.Keys.ModPath() + "\\Data\\" + item.Path, "perk__" + ToolRessources.Keys.ModId() + ".xml")))
				{
					writer.WriteLine($"    </perks>");
					writer.WriteLine($"</database>");
				}
			}

			return true;
		}

		public async Task<bool> WriteModManifest(ModDescription modDescription)
		{
			await Task.Yield();

			if (modDescription is null)
			{
				return false;
			}

			var path = ToolRessources.Keys.ModPath() + $"\\{modDescription.ModId}";

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
