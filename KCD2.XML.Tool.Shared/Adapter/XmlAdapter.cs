using KCD2.XML.Tool.Shared.Models;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace KCD2.XML.Tool.Shared.Adapter
{
	public class XmlAdapter : IXmlAdapter
	{
		private readonly string zipPath;
		private List<IModItem> modItems = new();

		public XmlAdapter(string zipPath)
		{
			this.zipPath = zipPath;
		}

		public async Task Initialize()
		{
			await Task.Yield();


			InitializePerks();
			InitializeBuffs();
			InitializeLocalizations();
		}

		private void InitializeLocalizations()
		{
			throw new NotImplementedException();
		}

		private void InitializeBuffs()
		{
			throw new NotImplementedException();
		}

		private void InitializePerks()
		{
			string path = string.Empty;

			using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var entry in archive.Entries)
				{

					path = Extensions.GetEntryPath(entry);

					if (entry.FullName.Contains("perk"))
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
	}
}
