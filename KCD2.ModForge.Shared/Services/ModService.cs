using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.Mods;
using Newtonsoft.Json;

namespace KCD2.ModForge.Shared.Services
{
	public class ModService
	{
		private ModDescription? mod = new();
		private ModCollection modCollection;
		private string modCollectionFile;
		private readonly JsonSerializerSettings settings = new()
		{
			TypeNameHandling = TypeNameHandling.All,
			Formatting = Formatting.Indented,
			PreserveReferencesHandling = PreserveReferencesHandling.None
		};

		public ModService()
		{
			modCollectionFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ModForge", "modcollection.json");
			Load();
		}

		public void Load()
		{
			if (File.Exists(modCollectionFile))
			{
				var json = File.ReadAllText(modCollectionFile);
				modCollection = JsonConvert.DeserializeObject<ModCollection>(json, settings)
						  ?? new ModCollection();
			}
			else
			{
				modCollection = new ModCollection();
			}
		}

		public void Save()
		{
			var json = JsonConvert.SerializeObject(modCollection, settings);

			Directory.CreateDirectory(Path.GetDirectoryName(modCollectionFile)!);
			File.WriteAllText(modCollectionFile, json);
			Load();
		}

		public ModDescription GetMod()
		{
			return mod!;
		}

		public ModCollection GetAllMods()
		{
			return modCollection;
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

		public void ClearMod()
		{
			mod = new();
		}

		public int GetAttributeCount()
		{
			if (mod is null)
			{
				return default;
			}
			return mod.ModItems.Count;
		}

		public bool TryGetMod(string modId)
		{
			if (string.IsNullOrEmpty(modId))
			{
				return false;
			}

			mod = modCollection.GetMod(modId);
			return true;
		}

		public async Task SaveMod(string name, string description, string author, string version, DateTime createdOn, string modId, bool modifiesLevel)
		{
			await Task.Yield();
			if (mod is null)
			{
				return;
			}

			mod.Name = name;
			mod.Description = description;
			mod.Author = author;
			mod.ModVersion = version;
			mod.CreatedOn = createdOn.ToString("yyyy-MM-dd");
			mod.ModId = modId;
			mod.ModifiesLevel = modifiesLevel;

			modCollection.AddMod(mod);
			Save();
		}

		public async Task<ModDescription> GenerateMod()
		{
			if (mod is null)
			{
				return null!;
			}

			//await adapter.WriteModManifest(mod);
			Save();
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

		public IEnumerable<IModItem> GetAll()
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

		public void RemoveMod(ModDescription mod)
		{
			modCollection.RemoveMod(mod);
			Save();
		}

		public async Task ExportMod()
		{
			//await adapter.WriteModItems(mod);
		}
	}
}
