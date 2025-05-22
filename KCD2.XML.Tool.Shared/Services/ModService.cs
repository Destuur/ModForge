using KCD2.XML.Tool.Shared.Adapter;
using KCD2.XML.Tool.Shared.Mods;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KCD2.XML.Tool.Shared.Services
{
	public class ModService
	{
		private ModDescription? mod = new();
		private readonly IXmlAdapter adapter;
		private readonly ModCollection modCollection;

		public ModService(IXmlAdapter adapter, ModCollection modCollection)
		{
			this.adapter = adapter;
			this.modCollection = modCollection;
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
			mod.CreatedOn = XmlConvert.ToString(createdOn, XmlDateTimeSerializationMode.Utc);
			mod.ModId = modId;
			mod.ModifiesLevel = modifiesLevel;

			modCollection.AddMod(mod);
		}

		public async Task GenerateMod()
		{
			if (mod is null)
			{
				return;
			}

			await adapter.WriteModManifest(mod);
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
	}
}
