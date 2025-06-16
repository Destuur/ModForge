using Microsoft.Extensions.Logging;
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
		#region Private Fields
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
		private readonly ILogger<ModService> logger;
		#endregion

		public ModService(
			DataSource dataSource,
			UserConfigurationService userConfigurationService,
			LocalizationService localizationService,
			ILogger<ModService> logger)
		{
			modCollectionFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ModForge", "modcollection.json");
			ReadModCollectionFromJson();
			this.dataSource = dataSource;
			this.userConfigurationService = userConfigurationService;
			this.localizationService = localizationService;
			this.logger = logger;
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

		#region Public Methods
		public void ReadExternalModsFromPak()
		{
			var modFolder = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods");
			var modDirectories = Directory.EnumerateDirectories(modFolder);

			foreach (var modPath in modDirectories)
			{
				try
				{
					var modFiles = Directory.GetFiles(modPath);
					if (modFiles.Length == 0)
						continue;

					var doc = XDocument.Load(modFiles.First());

					if (IsModCreatedByUser(doc))
						continue;

					var modDescription = ReadModMetadata(modPath);
					LoadModItems(modDescription);

					if (!externalModCollection.Any(x => x.ModId == modDescription.ModId))
						externalModCollection.Add(modDescription);
				}
				catch (Exception ex)
				{
					logger.LogWarning(ex, $"Mod in '{modPath}' konnte nicht gelesen werden.");
				}
			}
		}

		public void ReadModCollectionFromJson()
		{
			if (string.IsNullOrWhiteSpace(modCollectionFile))
			{
				logger.LogError("ReadModCollectionFromJson: The path to the mod collection file is null or empty.");
				modCollection = new ModCollection();
				return;
			}

			if (!File.Exists(modCollectionFile))
			{
				logger.LogInformation("Mod collection file not found. Creating a new empty collection.");
				modCollection = new ModCollection();
				return;
			}

			try
			{
				var json = File.ReadAllText(modCollectionFile);

				var deserialized = JsonConvert.DeserializeObject<ModCollection>(json, settings);

				if (deserialized == null)
				{
					logger.LogWarning("Deserialization returned null. Creating a new empty mod collection.");
					modCollection = new ModCollection();
				}
				else
				{
					modCollection = deserialized;
					logger.LogInformation("Mod collection successfully loaded from file.");
				}
			}
			catch (JsonException jex)
			{
				logger.LogError(jex, "Error deserializing the mod collection file.");
				modCollection = new ModCollection();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Unexpected error while loading the mod collection.");
				modCollection = new ModCollection();
			}
		}

		public bool WriteModCollectionAsJson()
		{
			try
			{
				if (string.IsNullOrWhiteSpace(modCollectionFile))
				{
					logger.LogError("WriteModCollectionAsJson: Path to file not valid.");
					return false;
				}

				var directory = Path.GetDirectoryName(modCollectionFile);
				if (string.IsNullOrWhiteSpace(directory))
				{
					logger.LogError("WriteModCollectionAsJson: Target directory not valid.");
					return false;
				}

				Directory.CreateDirectory(directory);

				var json = JsonConvert.SerializeObject(modCollection, settings);
				File.WriteAllText(modCollectionFile, json);

				logger.LogInformation("ModCollection was created in '{Path}'.", modCollectionFile);
				return true;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error occured while writing ModCollection.");
				return false;
			}
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

		public bool TryGetModFromCollection(string modId)
		{
			if (string.IsNullOrEmpty(modId))
			{
				logger.LogWarning("TryGetModFromCollection: modId is null or empty.");
				return false;
			}

			mod = modCollection.GetMod(modId);

			if (mod is null)
			{
				mod = externalModCollection.GetMod(modId);
			}

			if (mod is null)
			{
				logger.LogInformation("TryGetModFromCollection: No mod with ID '{ModId}' found.", modId);
				return false;
			}

			return true;
		}

		public ModDescription? CreateNewMod(string name, string description, string author, string version, DateTime createdOn, string modId, bool modifiesLevel)
		{
			if (string.IsNullOrWhiteSpace(name) ||
				string.IsNullOrWhiteSpace(modId) ||
				string.IsNullOrWhiteSpace(version))
			{
				logger.LogWarning("CreateNewMod: Required inputs missing.");
				return null;
			}

			if (modCollection.FirstOrDefault(x => x.ModId == modId) is not null)
			{
				logger.LogWarning("CreateNewMod: A mod with ID '{ModId}' already exists.", modId);
				return null;
			}

			var newMod = new ModDescription
			{
				Name = name,
				Description = description,
				Author = author,
				ModVersion = version,
				CreatedOn = createdOn.ToString("yyyy-MM-dd"),
				ModId = modId,
				ModifiesLevel = modifiesLevel
			};

			modCollection.AddMod(newMod);
			WriteModCollectionAsJson();

			logger.LogInformation("CreateNewMod: Mod '{Name}' with ID '{ModId}' created.", name, modId);

			return newMod;
		}

		public List<string> GetAllSupportedVersions()
		{
			return mod!.SupportsGameVersions;
		}

		public bool AddSupportedVersion(string version)
		{
			if (mod == null)
			{
				logger.LogWarning("AddSupportedVersion: Mod-Context is null.");
				return false;
			}

			if (string.IsNullOrWhiteSpace(version))
			{
				logger.LogWarning("AddSupportedVersion: Version is null or empty.");
				return false;
			}

			if (mod.SupportsGameVersions.Contains(version))
			{
				logger.LogInformation("AddSupportedVersion: Version '{Version}' already existing in the list.", version);
				return false;
			}

			mod.SupportsGameVersions.Add(version);
			logger.LogInformation("AddSupportedVersion: Version '{Version}' was added.", version);
			return true;
		}

		public bool RemoveSupportedVersion(string version)
		{
			if (mod == null)
			{
				logger.LogWarning("RemoveSupportedVersion: Mod-Context is null.");
				return false;
			}

			if (string.IsNullOrWhiteSpace(version))
			{
				logger.LogWarning("RemoveSupportedVersion: version is null or empty.");
				return false;
			}

			bool removed = mod.SupportsGameVersions.Remove(version);

			if (removed)
			{
				logger.LogInformation("Version '{Version}' successfully removed..", version);
			}
			else
			{
				logger.LogInformation("Version '{Version}' currently not in list.", version);
			}

			return removed;
		}

		public IEnumerable<IModItem> GetCurrentModItems()
		{
			return mod!.ModItems;
		}

		public bool AddModItem(IModItem item)
		{
			if (item == null)
			{
				logger.LogWarning("AddModItem: item is null.");
				return false;
			}

			if (mod == null)
			{
				logger.LogError("AddModItem: Mod-Context (mod) is null.");
				return false;
			}

			var existingItem = mod.ModItems.FirstOrDefault(x => x.Id == item.Id);

			if (existingItem != null)
			{
				mod.ModItems.Remove(existingItem);
				logger.LogInformation("ModItem with ID '{Id}' replaced.", item.Id);
			}
			else
			{
				logger.LogInformation("ModItem with ID '{Id}' added.", item.Id);
			}

			mod.ModItems.Add(item);
			return true;
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

		public void ExportMod(ModDescription mod)
		{
			var path = userConfigurationService.Current.GameDirectory;
			var pakPath = PathFactory.CreateModToPakPath(path, mod.ModId);
			WriteModManifest(mod);
			localizationService.WriteLocalizationAsXml(path, mod);

			dataSource.WriteModItems(mod.ModId, mod.ModItems);

			CreateModPak(pakPath, Path.Combine(pakPath, mod.ModId + ".pak"));
		}
		#endregion

		#region Private Methods
		private ModDescription ReadModMetadata(string modPath)
		{
			var modFiles = Directory.GetFiles(modPath);
			if (modFiles.Length == 0)
				throw new FileNotFoundException($"Keine Dateien im Mod-Ordner: {modPath}");

			var modFileUri = modFiles.FirstOrDefault();
			if (string.IsNullOrEmpty(modFileUri))
				throw new FileNotFoundException($"Mod-Info-Datei fehlt in {modPath}");

			var doc = XDocument.Load(modFileUri);
			var root = doc.Root ?? throw new InvalidDataException("Mod-Datei hat keine Root");

			var info = root.Element("info") ?? throw new InvalidDataException("Element <info> fehlt");
			var modifiesLevel = bool.TryParse(info.Element("modifies_level")?.Value, out var modifies) && modifies;

			var supports = root.Element("supports") ?? throw new InvalidDataException("Element <supports> fehlt");
			var supportList = supports.Elements("kcd_version").Select(e => e.Value).ToList();

			return new ModDescription
			{
				Name = info.Element("name")?.Value,
				Description = info.Element("description")?.Value,
				Author = info.Element("author")?.Value,
				ModVersion = info.Element("version")?.Value,
				CreatedOn = info.Element("created_on")?.Value,
				ModId = info.Element("modid")?.Value,
				ModifiesLevel = modifiesLevel,
				SupportsGameVersions = supportList
			};
		}

		private bool IsModCreatedByUser(XDocument doc)
		{
			var author = doc.Root?.Element("info")?.Element("author")?.Value;
			return !string.IsNullOrEmpty(author) && author == userConfigurationService.Current.UserName;
		}

		private void LoadModItems(ModDescription modDescription)
		{
			var dataFolder = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", modDescription.ModId, "Data");
			var pakFile = Path.Combine(dataFolder, modDescription.ModId + ".pak");

			var dataPoints = new List<IDataPoint>
			{
				new DataPoint(pakFile, "perk", typeof(Perk)),
				new DataPoint(pakFile, "buff", typeof(Buff))
			};

			foreach (var dataPoint in dataPoints)
			{
				var modItems = dataSource.ReadModItems(dataPoint)
								?? throw new NullReferenceException("Fehler beim Lesen von ModItems");

				foreach (var item in modItems)
					modDescription.ModItems.Add(item);
			}
		}

		private bool IsValidModDescription(ModDescription mod)
		{
			return mod != null
				&& !string.IsNullOrWhiteSpace(mod.ModId)
				&& !string.IsNullOrWhiteSpace(mod.Name);
		}

		private bool WriteModManifest(ModDescription mod)
		{
			if (IsValidModDescription(mod) == false)
			{
				logger.LogWarning("ModDescription is null – manifest wont be created.");
				return false;
			}

			try
			{
				var modRootPath = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", mod.ModId);
				var dataPath = Path.Combine(modRootPath, "Data");
				var localizationPath = Path.Combine(modRootPath, "Localization");
				var manifestPath = Path.Combine(modRootPath, "mod.manifest");

				Directory.CreateDirectory(dataPath);
				Directory.CreateDirectory(localizationPath);

				if (File.Exists(manifestPath))
				{
					logger.LogInformation("Manifest already exists: {ManifestPath}", manifestPath);
					return true;
				}

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
							new XElement("modifies_level", mod.ModifiesLevel.ToString().ToLowerInvariant())
						),
						new XElement("supports",
							mod.SupportsGameVersions.Select(v => new XElement("kcd_version", v))
						)
					)
				);

				doc.Save(manifestPath);
				logger.LogInformation("Manifest successfully created: {ManifestPath}", manifestPath);
				return true;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occured while writing mod.manifest: {ModId}", mod?.ModId);
				return false;
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

		private void CreateModPak(string baseFolder, string pakFilePath)
		{
			try
			{
				if (File.Exists(pakFilePath))
				{
					File.Delete(pakFilePath);
					logger.LogInformation("Bestehende PAK-Datei gelöscht: {PakFile}", pakFilePath);
				}

				var outputFolder = Path.GetDirectoryName(pakFilePath);
				if (!string.IsNullOrEmpty(outputFolder) && !Directory.Exists(outputFolder))
				{
					Directory.CreateDirectory(outputFolder);
					logger.LogInformation("Zielverzeichnis erstellt: {OutputFolder}", outputFolder);
				}

				using var fs = new FileStream(pakFilePath, FileMode.CreateNew);
				using var archive = new ZipArchive(fs, ZipArchiveMode.Create);

				var files = Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories);

				foreach (var file in files)
				{
					if (Path.GetFullPath(file) == Path.GetFullPath(pakFilePath))
						continue;

					AddFileToArchive(archive, file, baseFolder);
				}

				logger.LogInformation("PAK-Datei erfolgreich erstellt: {PakFile}", pakFilePath);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Fehler beim Erstellen der PAK-Datei: {PakFile}", pakFilePath);
				throw; // ggf. entfernen oder weiterreichen, je nach Anforderung
			}
		}

		private void AddFileToArchive(ZipArchive archive, string filePath, string baseFolder)
		{
			var relativePath = Path.GetRelativePath(baseFolder, filePath).Replace('\\', '/');
			var entry = archive.CreateEntry(relativePath, CompressionLevel.NoCompression);

			using var entryStream = entry.Open();
			using var fileStream = File.OpenRead(filePath);
			fileStream.CopyTo(entryStream);

			logger.LogDebug("Datei hinzugefügt: {Path}", relativePath);
		}
		#endregion
	}
}
