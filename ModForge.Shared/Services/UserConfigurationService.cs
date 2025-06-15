using ModForge.Shared.Models.User;
using System.Text.Json;

namespace ModForge.Shared.Services
{
	public class UserConfigurationService
	{
		private readonly string configFile;
		private JsonSerializerOptions jsonSettings = new JsonSerializerOptions()
		{
			WriteIndented = true
		};

		public UserConfigurationService()
		{
			configFile = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"ModForge", "userconfig.json");

			Load();
		}

		public UserConfiguration? Current { get; set; }

		private void Load()
		{
			if (File.Exists(configFile))
			{
				var json = File.ReadAllText(configFile);
				Current = JsonSerializer.Deserialize<UserConfiguration>(json)
						  ?? new UserConfiguration();
			}
			else
			{
				Current = new UserConfiguration();
			}
		}

		public void Save()
		{
			var json = JsonSerializer.Serialize(Current, jsonSettings);
			Directory.CreateDirectory(Path.GetDirectoryName(configFile)!);
			File.WriteAllText(configFile, json);
		}
	}
}
