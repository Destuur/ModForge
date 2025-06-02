using Newtonsoft.Json;

namespace KCD2.ModForge.Shared.Adapter
{
	public class JsonAdapterOfT<T> : IModItemAdapter<T>
	{
		private readonly List<T> data;
		private readonly string configFile;
		private readonly JsonSerializerSettings settings = new()
		{
			TypeNameHandling = TypeNameHandling.All,
			Formatting = Formatting.Indented,
			PreserveReferencesHandling = PreserveReferencesHandling.None
		};

		public JsonAdapterOfT()
		{
			//var json = File.ReadAllText(filePath);
			//data = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
			configFile = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"ModForge", $"{typeof(T).Name.ToLower()}s.json");
		}

		public void Initialize()
		{
			throw new NotImplementedException();
		}
		public void Deinitialize()
		{
			throw new NotImplementedException();
		}

		public IList<T> ReadModItems(string filePath)
		{
			if (File.Exists(filePath))
			{
				var type = typeof(T);
				var json = File.ReadAllText(filePath);
				var modItemList = JsonConvert.DeserializeObject<IList<T>>(json, settings)
						  ?? new List<T>();
				return modItemList;
			}
			else
			{
				return new List<T>();
			}
		}

		public bool WriteModItems(IEnumerable<T> modItems)
		{
			var json = JsonConvert.SerializeObject(modItems, settings);
			//var json = JsonSerializer.Serialize(modItems, new JsonSerializerOptions
			//{
			//	WriteIndented = true
			//});

			Directory.CreateDirectory(Path.GetDirectoryName(configFile)!);
			File.WriteAllText(configFile, json);
			return true;
		}

		public T GetModItem(string id)
		{
			throw new NotImplementedException();
		}

		public bool WriteModItem(T modItem)
		{
			throw new NotImplementedException();
		}
	}
}
