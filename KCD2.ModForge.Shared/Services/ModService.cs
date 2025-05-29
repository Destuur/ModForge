using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Mods;

namespace KCD2.ModForge.Shared.Services
{
	public class ModService
	{
		private ModDescription? mod = new();
		private readonly ModCollection modCollection;

		public ModService(ModCollection modCollection)
		{
			this.modCollection = modCollection;
		}

		public ModDescription GetMod()
		{
			return mod!;
		}

		public bool TrySetMod(string modId)
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
		}

		public async Task<ModDescription> GenerateMod()
		{
			if (mod is null)
			{
				return null!;
			}

			//await adapter.WriteModManifest(mod);
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

		public bool AddItem(IModItem item)
		{
			if (item is null)
			{
				return false;
			}

			if (mod!.ModItems.Contains(item))
			{
				return false;
			}

			mod!.ModItems.Add(item);
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

		public async Task ExportMod()
		{
			//await adapter.WriteModItems(mod);
		}
	}
}
