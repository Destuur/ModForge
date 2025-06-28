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
		private ModDescription mod = new();
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
			this.dataSource = dataSource;
			this.userConfigurationService = userConfigurationService;
			this.localizationService = localizationService;
			this.logger = logger;
			InitiateModCollections();
		}

		public ModDescription Mod
		{
			get => mod;
			private set
			{
				if (value is null)
				{
					return;
				}
				mod = value;
			}
		}
		public ModCollection ModCollection { get; private set; } = new();
		public ModCollection ExternalModCollection { get; private set; } = new();

		#region Public Methods
		public void InitiateModCollections()
		{
			if (userConfigurationService.Current is null)
			{
				logger.LogWarning($"'{userConfigurationService.Current}' is no valid user configuration.");
				return;
			}

			if (string.IsNullOrEmpty(userConfigurationService.Current.GameDirectory))
			{
				logger.LogWarning($"'{userConfigurationService.Current.GameDirectory}' is no valid directory path.");
				return;
			}

			var modFolder = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods");
			var modDirectories = Directory.EnumerateDirectories(modFolder);

			ModCollection.Clear();
			ExternalModCollection.Clear();

			foreach (var modPath in modDirectories)
			{
				try
				{
					var modFiles = Directory.GetFiles(modPath);
					if (modFiles.Length == 0)
					{
						continue;
					}

					var doc = XDocument.Load(modFiles.First());

					if (IsModCreatedByUser(doc))
					{
						FillModCollection(modPath, ModCollection);
					}
					else
					{
						FillModCollection(modPath, ExternalModCollection);
					}

				}
				catch (Exception ex)
				{
					logger.LogWarning(ex, $"Mod in '{modPath}' konnte nicht gelesen werden.");
				}
			}
		}

		private void FillModCollection(string modPath, ModCollection collection)
		{
			var modDescription = ReadModMetadata(modPath);
			LoadModItems(modDescription);

			if (!collection.Any(x => x.Id == modDescription.Id))
			{
				collection.Add(modDescription);
			}
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

			mod = ModCollection.GetMod(modId);

			if (mod is null)
			{
				mod = ExternalModCollection.GetMod(modId);
			}

			if (mod is null)
			{
				logger.LogInformation("TryGetModFromCollection: No mod with ID '{ModId}' found.", modId);
				return false;
			}

			return true;
		}

		public ModDescription CreateNewMod(string name, string description, string author, string version, DateTime createdOn, string modId, bool modifiesLevel, List<string> supportedGameVersions)
		{
			if (string.IsNullOrWhiteSpace(name) ||
				string.IsNullOrWhiteSpace(modId) ||
				string.IsNullOrWhiteSpace(version))
			{
				logger.LogWarning("CreateNewMod: Required inputs missing.");
				return new ModDescription();
			}

			if (ModCollection.FirstOrDefault(x => x.Id == modId) is not null)
			{
				logger.LogWarning("CreateNewMod: A mod with ID '{ModId}' already exists.", modId);
				return new ModDescription();
			}

			var newMod = new ModDescription
			{
				Name = name,
				Description = description,
				Author = author,
				ModVersion = version,
				CreatedOn = createdOn.ToString("yyyy-MM-dd"),
				Id = modId,
				ModifiesLevel = modifiesLevel,
				SupportsGameVersions = supportedGameVersions
			};

			ModCollection.AddMod(newMod);
			logger.LogInformation("CreateNewMod: Mod '{Name}' with ID '{ModId}' created.", name, modId);
			return newMod;
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

		public void DeleteMod(ModDescription mod)
		{
			if (userConfigurationService.Current is null)
			{
				logger.LogWarning($"'{userConfigurationService.Current}' is no valid user configuration.");
				return;
			}

			var modFolder = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods");
			var modDirectories = Directory.EnumerateDirectories(modFolder);

			foreach (var modPath in modDirectories)
			{
				try
				{


					var modFiles = Directory.GetFiles(modPath);
					if (modFiles.Length == 0)
					{
						continue;
					}

					var doc = XDocument.Load(modFiles.First());

					if (IsModCreatedByUser(doc))
					{
						FillModCollection(modPath, ModCollection);
					}
					else
					{
						FillModCollection(modPath, ExternalModCollection);
					}

				}
				catch (Exception ex)
				{
					logger.LogWarning(ex, $"Mod in '{modPath}' konnte nicht gelesen werden.");
				}
			}
		}

		public void DeleteMods(IEnumerable<ModDescription> mod)
		{
			//ModCollection.RemoveMod(mod);
		}

		public void RemoveModFromExternalCollection(ModDescription mod)
		{
			ExternalModCollection.RemoveMod(mod);
		}

		public void ExportMod(ModDescription mod)
		{
			var path = userConfigurationService.Current.GameDirectory;
			var pakPath = PathFactory.CreateModToPakPath(path, mod.Id);
			WriteModManifest(mod);
			localizationService.WriteLocalizationAsXml(path, mod);

			dataSource.WriteModItems(mod.Id, mod.ModItems);

			CreateModPak(pakPath, Path.Combine(pakPath, mod.Id + ".pak"));
		}
		#endregion

		#region Private Methods
		private ModDescription ReadModMetadata(string modPath)
		{
			logger?.LogInformation("Reading mod metadata from path: {ModPath}", modPath);

			var modFiles = Directory.GetFiles(modPath);
			if (modFiles.Length == 0)
			{
				var msg = $"No files found in mod folder: {modPath}";
				logger?.LogError(msg);
				throw new FileNotFoundException(msg);
			}

			var modFileUri = modFiles.FirstOrDefault();
			if (string.IsNullOrEmpty(modFileUri))
			{
				var msg = $"Mod info file missing in {modPath}";
				logger?.LogError(msg);
				throw new FileNotFoundException(msg);
			}

			XDocument doc;
			try
			{
				doc = XDocument.Load(modFileUri);
				logger?.LogInformation("Mod XML loaded from file: {ModFile}", modFileUri);
			}
			catch (Exception ex)
			{
				logger?.LogError(ex, "Failed to load XML from mod file: {ModFile}", modFileUri);
				throw;
			}

			var root = doc.Root ?? throw new InvalidDataException("Mod file has no root element");
			var info = root.Element("info") ?? throw new InvalidDataException("Missing <info> element");

			var modifiesLevel = bool.TryParse(info.Element("modifies_level")?.Value, out var modifies) && modifies;

			List<string> supportList = new List<string>();
			try
			{
				var supports = root.Element("supports");
				if (supports != null)
				{
					supportList = supports.Elements("kcd_version").Select(e => e.Value).ToList();
					logger?.LogInformation("Found {Count} supported game versions", supportList.Count);
				}
				else
				{
					logger?.LogWarning("No <supports> element found in mod metadata");
				}
			}
			catch (Exception ex)
			{
				logger?.LogError(ex, "Error reading supported game versions from mod metadata");
				supportList = new List<string>();
			}

			var modDescription = new ModDescription
			{
				Name = info.Element("name")?.Value,
				Description = info.Element("description")?.Value,
				Author = info.Element("author")?.Value,
				ModVersion = info.Element("version")?.Value,
				CreatedOn = info.Element("created_on")?.Value,
				Id = info.Element("modid")?.Value,
				ModifiesLevel = modifiesLevel,
				SupportsGameVersions = supportList
			};

			logger?.LogInformation("Mod metadata successfully read: {ModId}", modDescription.Id);
			return modDescription;
		}


		private bool IsModCreatedByUser(XDocument doc)
		{
			var author = doc.Root?.Element("info")?.Element("author")?.Value;
			return !string.IsNullOrEmpty(author) && author == userConfigurationService.Current.UserName;
		}

		private void LoadModItems(ModDescription modDescription)
		{
			var dataFolder = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", modDescription.Id, "Data");
			var pakFile = Path.Combine(dataFolder, modDescription.Id + ".pak");

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
				&& !string.IsNullOrWhiteSpace(mod.Id)
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
				var modRootPath = Path.Combine(userConfigurationService.Current.GameDirectory, "Mods", mod.Id);
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
							new XElement("modid", mod.Id),
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
				logger.LogError(ex, "An error occured while writing mod.manifest: {ModId}", mod?.Id);
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
