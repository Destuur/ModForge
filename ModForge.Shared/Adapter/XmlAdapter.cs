using ModForge.Shared.Factories;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Data;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using System.IO.Compression;
using System.Xml.Linq;

namespace ModForge.Shared.Adapter
{
	public partial class XmlAdapter : IModItemAdapter
	{
		private readonly UserConfigurationService userConfigurationService;

		public XmlAdapter(UserConfigurationService userConfigurationService)
		{
			this.userConfigurationService = userConfigurationService;
		}

		public IList<IModItem> ReadModItems(IDataPoint dataPoint)
		{
			var filePath = dataPoint.Path;
			var foundModItems = new List<IModItem>();
			var type = dataPoint.Type.Name.ToString().ToLower();

			using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var entry in archive.Entries)
				{
					if (entry.FullName.EndsWith(".tbl"))
					{
						continue;
					}

					if (entry.FullName.Contains(dataPoint.Endpoint))
					{
						using (Stream stream = entry.Open())
						{
							try
							{
								XDocument doc = XDocument.Load(stream);

								foreach (var perkElement in doc.Descendants(type))
								{
									var modItem = ModItemFactory.CreateModItem(perkElement, dataPoint.Type, entry.FullName);

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

			return GetLinkedModItem(filePath, foundModItems);
		}

		private IList<IModItem> GetLinkedModItem(string filePath, IList<IModItem> modItems)
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
									var perk = modItems.FirstOrDefault(x => x.Id == perkId);
									var buff = modItems.FirstOrDefault(x => x.Id == buffId);
									// TODO: Als eigene Property oder als Attribut hinzufügen?
									if (perk is not null)
									{
										perk.LinkedIds.Add(buffId);
									}
									if (buff is not null)
									{
										buff.LinkedIds.Add(perkId);
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
			return modItems;
		}

		public void WriteModItems(string modId, IEnumerable<IModItem> modItems)
		{
			foreach (var modItem in modItems)
			{
				WriteModItem(modId, modItem);
			}
			var pakPath = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", modId, "Data");
		}


		private bool WriteModItem(string modId, IModItem modItem)
		{
			string directoryUpToRpg = modItem.Path.Substring(0, modItem.Path.IndexOf("rpg") + "rpg".Length);
			var modItemDirectory = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", modId, "Data", directoryUpToRpg);
			var modItemFile = Path.Combine(modItemDirectory, modItem.GetType().Name.ToLower() + "__" + modId + ".xml");

			Directory.CreateDirectory(modItemDirectory);

			if (File.Exists(modItemFile))
				File.Delete(modItemFile);

			var attributes = new List<XAttribute>();

			foreach (var kv in modItem.Attributes)
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

			var modItemElement = new XElement(modItem.GetType().Name.ToLower(), attributes);

			var doc = new XDocument(
				new XDeclaration("1.0", "us-ascii", null),
				new XElement("database",
					new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
					new XAttribute("name", "barbora"),
					new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../database.xsd"),
					new XElement(modItem.GetType().Name.ToLower() + "s",
						new XAttribute("version", "1"),
						modItemElement
					)
				)
			);

			doc.Save(modItemFile);
			return true;
		}
	}
}
