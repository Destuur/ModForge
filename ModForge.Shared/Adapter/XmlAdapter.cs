using ModForge.Shared.Builders;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Mapping;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;

namespace ModForge.Shared.Adapter
{
	public partial class XmlAdapter : IModItemAdapter
	{
		private readonly UserConfigurationService userConfigurationService;
		private readonly IBuilder<XElement, IModItem> builder;

		public XmlAdapter(UserConfigurationService userConfigurationService, IBuilder<XElement, IModItem> builder)
		{
			this.userConfigurationService = userConfigurationService;
			this.builder = builder;
		}

		public IList<IModItem> ReadModItems(IDataPoint dataPoint)
		{
			var filePath = dataPoint.Path;
			var foundModItems = new List<IModItem>();
			var type = dataPoint.Type.Name.ToString();

			using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
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

								AttributeFactory.DiscoverAndAddAttributeTypes(doc);

								var descandants = doc.Descendants(type);

								foreach (var element in doc.Descendants().Where(e => e.Name.LocalName.Equals(type, StringComparison.OrdinalIgnoreCase)))
								{
									var modItem = builder.Build(element);
									if (modItem != null)
									{
										modItem.Path = entry.FullName;
										foundModItems.Add(modItem);
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

			//return GetLinkedModItem(filePath, foundModItems);
			return foundModItems;
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
			string typeName = modItem.GetType().Name;

			if (!XmlStructureMapping.ElementMapping.TryGetValue(typeName, out var writeInfo))
			{
				Console.WriteLine($"Kein Mapping für Typ '{typeName}' gefunden.");
				return false;
			}

			string fileName = $"{writeInfo.FilePrefix}__{modId}.xml";
			string directoryUpToRpg = modItem.Path.Substring(0, modItem.Path.LastIndexOf('/')); // oder mit LastIndexOf('\\')
			string modItemDirectory = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", modId, "Data", directoryUpToRpg);
			string modItemFile = Path.Combine(modItemDirectory, fileName);

			Directory.CreateDirectory(modItemDirectory);

			// Neues Element generieren
			var attributes = modItem.Attributes.Select(kv =>
			{
				object value = kv.Value;

				if (kv.Name == "buff_params" && value is List<BuffParam> buffList)
					return new XAttribute(kv.Name, BuffParamSerializer.ToAttributeString(buffList));

				if (value is Enum enumValue)
					return new XAttribute(kv.Name, Convert.ToInt32(enumValue));

				if (value is bool boolValue)
					return new XAttribute(kv.Name, boolValue.ToString().ToLowerInvariant());

				if (value is IFormattable formattable)
					return new XAttribute(kv.Name, formattable.ToString(null, CultureInfo.InvariantCulture));

				return new XAttribute(kv.Name, value?.ToString() ?? string.Empty);
			});

			var newElement = new XElement(writeInfo.ElementName, attributes);

			XDocument doc;

			// Datei existiert bereits? Dann laden und ergänzen
			if (File.Exists(modItemFile))
			{
				doc = XDocument.Load(modItemFile);

				var group = doc.Descendants(writeInfo.GroupName).FirstOrDefault();
				if (group == null)
				{
					Console.WriteLine($"Warnung: Gruppe '{writeInfo.GroupName}' nicht gefunden in {modItemFile}. Neue Gruppe wird angelegt.");
					group = new XElement(writeInfo.GroupName, new XAttribute("version", "1"));
					doc.Root?.Add(group);
				}

				var idKey = modItem.IdKey;

				if (!string.IsNullOrWhiteSpace(idKey))
				{
					var idValue = newElement.Attribute(idKey)?.Value;

					if (idValue != null)
					{
						var existingElement = group.Elements(writeInfo.ElementName)
							.FirstOrDefault(el =>
								string.Equals(el.Attribute(idKey)?.Value, idValue, StringComparison.Ordinal));

						if (existingElement != null)
						{
							existingElement.ReplaceWith(newElement);
						}
						else
						{
							group.Add(newElement);
						}
					}
					else
					{
						Console.WriteLine($"Warnung: Attribut '{idKey}' im neuen Element nicht gefunden. Element wird hinzugefügt.");
						group.Add(newElement);
					}
				}
				else
				{
					Console.WriteLine($"Warnung: Kein gültiger 'IdKey' für Typ '{modItem.GetType().Name}' gesetzt. Element wird hinzugefügt.");
					group.Add(newElement);
				}

			}
			else
			{
				// Neue Datei anlegen
				doc = new XDocument(
					new XDeclaration("1.0", "us-ascii", null),
					new XElement("database",
						new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
						new XAttribute("name", "barbora"),
						new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../database.xsd"),
						new XElement(writeInfo.GroupName,
							new XAttribute("version", "1"),
							newElement
						)
					)
				);
			}

			doc.Save(modItemFile);
			return true;
		}

		private static bool ElementsAreEqual(XElement a, XElement b)
		{
			// Beide null? Gleich
			if (a == null && b == null) return true;
			if (a == null || b == null) return false;

			// Unterschiedlicher Elementname?
			if (!string.Equals(a.Name.LocalName, b.Name.LocalName, StringComparison.OrdinalIgnoreCase))
				return false;

			var attrsA = a.Attributes().ToDictionary(attr => attr.Name.LocalName.ToLowerInvariant(), attr => attr.Value);
			var attrsB = b.Attributes().ToDictionary(attr => attr.Name.LocalName.ToLowerInvariant(), attr => attr.Value);

			// Gleiche Anzahl an Attributen?
			if (attrsA.Count != attrsB.Count)
				return false;

			// Alle Attribute gleich?
			foreach (var kv in attrsA)
			{
				if (!attrsB.TryGetValue(kv.Key, out var valueB))
					return false;

				if (kv.Value != valueB)
					return false;
			}

			return true;
		}



		//private bool WriteModItem(string modId, IModItem modItem)
		//{
		//	string directoryUpToRpg = modItem.Path.Substring(0, modItem.Path.IndexOf("rpg") + "rpg".Length);
		//	var modItemDirectory = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", modId, "Data", directoryUpToRpg);
		//	var modItemFile = Path.Combine(modItemDirectory, modItem.GetType().Name.ToLower() + "__" + modId + ".xml");

		//	Directory.CreateDirectory(modItemDirectory);

		//	if (File.Exists(modItemFile))
		//		File.Delete(modItemFile);

		//	var attributes = new List<XAttribute>();

		//	foreach (var kv in modItem.Attributes)
		//	{
		//		if (kv.Name == "buff_params" && kv.Value is List<BuffParam> list)
		//		{
		//			string serialized = BuffParamSerializer.ToAttributeString(list);
		//			attributes.Add(new XAttribute(kv.Name, serialized));
		//		}
		//		else if (kv.Value is Enum enumValue)
		//		{
		//			attributes.Add(new XAttribute(kv.Name, Convert.ToInt32(enumValue)));
		//		}
		//		else if (kv.Value is bool boolValue)
		//		{
		//			attributes.Add(new XAttribute(kv.Name, boolValue.ToString().ToLower()));
		//		}
		//		else
		//		{
		//			attributes.Add(new XAttribute(kv.Name, kv.Value?.ToString() ?? string.Empty));
		//		}
		//	}

		//	var modItemElement = new XElement(modItem.GetType().Name.ToLower(), attributes);

		//	var doc = new XDocument(
		//		new XDeclaration("1.0", "us-ascii", null),
		//		new XElement("database",
		//			new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
		//			new XAttribute("name", "barbora"),
		//			new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../database.xsd"),
		//			new XElement(modItem.GetType().Name.ToLower() + "s",
		//				new XAttribute("version", "1"),
		//				modItemElement
		//			)
		//		)
		//	);

		//	doc.Save(modItemFile);
		//	return true;
		//}
	}
}
