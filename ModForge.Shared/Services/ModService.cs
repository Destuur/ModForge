using ModForge.Shared.Adapter;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.Data;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Xml.Linq;

namespace ModForge.Shared.Services
{
	public class ModService
	{
		private ModDescription? mod = new();
		private ModCollection modCollection = new();
		private ModCollection externalModCollection = new();
		private string modCollectionFile;
		private readonly JsonSerializerSettings settings = new()
		{
			TypeNameHandling = TypeNameHandling.All,
			Formatting = Formatting.Indented,
			PreserveReferencesHandling = PreserveReferencesHandling.None
		};
		private readonly DataSource dataSource;
		private readonly UserConfigurationService userConfigurationService;
		private readonly LocalizationService localizationService;

		public ModService(DataSource dataSource, UserConfigurationService userConfigurationService, LocalizationService localizationService)
		{
			modCollectionFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ModForge", "modcollection.json");
			ReadModCollectionFromJson();
			this.dataSource = dataSource;
			this.userConfigurationService = userConfigurationService;
			this.localizationService = localizationService;
		}

		public ModDescription Mod
		{
			get => mod;
			set
			{
				if (value is null)
				{
					return;
				}
				mod = value;
			}
		}

		public void ReadExternalModsFromPak()
		{
			var modFolder = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods");
			var files = Directory.EnumerateDirectories(modFolder);

			foreach (var file in files)
			{
				var modFiles = Directory.GetFiles(file);

				var doc = XDocument.Load(modFiles.FirstOrDefault());

				var info = doc.Root.Element("info");

				if (info.Element("author")?.Value.Equals(userConfigurationService.Current.UserName) == true)
				{
					continue;
				}

				var supports = doc.Root.Element("supports");
				var parseElement = bool.TryParse(info.Element("modifies_level").Value, out bool result);
				var supportList = new List<string>();
				var supportingVersionElements = supports.Elements("kcd_version");

				foreach (var supportingVersionElement in supportingVersionElements)
				{
					supportList.Add(supportingVersionElement.Value);
				}

				var modDescription = new ModDescription()
				{
					Name = info.Element("name")?.Value,
					Description = info.Element("description")?.Value,
					Author = info.Element("author")?.Value,
					ModVersion = info.Element("version")?.Value,
					CreatedOn = info.Element("created_on")?.Value,
					ModId = info.Element("modid")?.Value,
					ModifiesLevel = result,
					SupportsGameVersions = supportList
				};

				var folder = Path.Combine(file, "Data");
				var pak = Path.Combine(folder, modDescription.ModId + ".pak");

				var dataPoints = new List<IDataPoint>()
				{
					new DataPoint(pak, "perk", typeof(Perk)),
					new DataPoint(pak, "buff", typeof(Buff))
				};

				foreach (var dataPoint in dataPoints)
				{
					var modItems = dataSource.ReadModItems(dataPoint);

					if (modItems is null)
					{
						throw new NullReferenceException();
					}

					foreach (var modItem in modItems)
					{
						modDescription.ModItems.Add(modItem);
					}
				}

				if (externalModCollection.FirstOrDefault(x => x.ModId == modDescription.ModId) is null)
				{
					externalModCollection.Add(modDescription);
				}
			}
		}

		public void ReadModCollectionFromJson()
		{
			if (File.Exists(modCollectionFile))
			{
				var json = File.ReadAllText(modCollectionFile);
				modCollection = JsonConvert.DeserializeObject<ModCollection>(json, settings) ?? new ModCollection();
			}
			else
			{
				modCollection = new ModCollection();
			}
		}

		public void WriteModCollectionAsJson()
		{
			var json = JsonConvert.SerializeObject(modCollection, settings);

			Directory.CreateDirectory(Path.GetDirectoryName(modCollectionFile)!);
			File.WriteAllText(modCollectionFile, json);
			ReadModCollectionFromJson();
		}

		public ModDescription GetCurrentMod()
		{
			return mod!;
		}

		public ModCollection GetAllMods()
		{
			return modCollection;
		}

		public ModCollection GetAllExternalMods()
		{
			return externalModCollection;
		}

		public void ClearCurrentMod()
		{
			mod = new();
		}

		public int GetCurrentAttributeCount()
		{
			if (mod is null)
			{
				return default;
			}
			return mod.ModItems.Count;
		}

		public bool TryGetModFromCollection(string modId)
		{
			if (string.IsNullOrEmpty(modId))
			{
				return false;
			}

			mod = modCollection.GetMod(modId);

			if (mod is null)
			{
				mod = externalModCollection.GetMod(modId);
			}

			if (mod is null)
			{
				throw new NullReferenceException();
			}

			return true;
		}

		public ModDescription CreateNewMod(string name, string description, string author, string version, DateTime createdOn, string modId, bool modifiesLevel)
		{
			if (mod is null)
			{
				return new ModDescription();
			}

			mod.Name = name;
			mod.Description = description;
			mod.Author = author;
			mod.ModVersion = version;
			mod.CreatedOn = createdOn.ToString("yyyy-MM-dd");
			mod.ModId = modId;
			mod.ModifiesLevel = modifiesLevel;

			modCollection.AddMod(mod);
			WriteModCollectionAsJson();
			return mod;
		}

		public List<string> GetAllSupportedVersions()
		{
			return mod!.SupportsGameVersions;
		}

		public bool AddSupportedVersion(string version)
		{
			if (mod is null)
			{
				return false;
			}

			mod.SupportsGameVersions.Add(version);
			return true;
		}

		public bool RemoveSupportedVersion(string version)
		{
			if (mod is null)
			{
				return false;
			}

			mod.SupportsGameVersions.Remove(version);
			return true;
		}

		public IEnumerable<IModItem> GetCurrentModItems()
		{
			return mod!.ModItems;
		}

		public bool AddModItem(IModItem item)
		{
			if (item is null)
			{
				return false;
			}

			if (mod!.ModItems.FirstOrDefault(x => x.Id == item.Id) is null)
			{
				mod!.ModItems.Add(item);
				return true;
			}
			else
			{
				var oldItem = mod!.ModItems.FirstOrDefault(x => x.Id == item.Id);
				mod.ModItems.Remove(oldItem!);
				mod.ModItems.Add(item);
				return true;
			}
		}

		public async Task AddModIcon(string path)
		{
			await Task.Yield();
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			mod!.ImagePath = path;
		}

		public void ClearModCollection()
		{
			modCollection.Clear();
		}

		public void RemoveModFromCollection(ModDescription mod)
		{
			modCollection.RemoveMod(mod);
			WriteModCollectionAsJson();
		}

		public void RemoveModFromExternalCollection(ModDescription mod)
		{
			externalModCollection.RemoveMod(mod);
		}

		private bool WriteModManifest(ModDescription mod)
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

		public void ExportMod(ModDescription mod)
		{
			var path = userConfigurationService.Current.GameDirectory;
			var pakPath = PathFactory.CreateModToPakPath(path, mod.ModId);
			WriteModManifest(mod);
			localizationService.WriteLocalizationAsXml(path, mod);

			dataSource.WriteModItems(mod.ModId, mod.ModItems);

			CreateModPak(pakPath, Path.Combine(pakPath, mod.ModId + ".pak"));
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

		public void SetCurrentMod(ModDescription? mod)
		{
			if (mod is null)
			{
				return;
			}

			this.mod = mod;
		}
	}
}
