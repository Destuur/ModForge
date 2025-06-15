using ModForge.Shared.Models.Data;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using Newtonsoft.Json;

namespace ModForge.Shared.Adapter
{
	public class JsonAdapter
	{
		private readonly string configFile;
		private readonly JsonSerializerSettings settings = new()
		{
			TypeNameHandling = TypeNameHandling.All,
			Formatting = Formatting.Indented,
			PreserveReferencesHandling = PreserveReferencesHandling.None
		};

		public JsonAdapter()
		{
			configFile = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"ModForge");
		}

		public IList<IModItem> ReadModItemsFromJson(string path)
		{
			if (File.Exists(path))
			{
				var json = File.ReadAllText(path);
				var modItemList = JsonConvert.DeserializeObject<IList<IModItem>>(json, settings)
						  ?? new List<IModItem>();
				return modItemList;
			}
			else
			{
				return new List<IModItem>();
			}
		}

		public void WriteModItemsAsJson(IEnumerable<IModItem> modItems)
		{
			var json = JsonConvert.SerializeObject(modItems, settings);
			var jsonFile = Path.Combine(configFile, modItems.FirstOrDefault().GetType().Name.ToLower() + "s.json");

			Directory.CreateDirectory(Path.GetDirectoryName(jsonFile)!);
			File.WriteAllText(jsonFile, json);
		}
	}
}
