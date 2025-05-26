using KCD2.ModForge.Shared.Models.User;
using System.Text.Json;

namespace KCD2.ModForge.Shared.Services
{
	public class UserConfigurationService
	{
		private readonly string configFile;

		public UserConfigurationService()
		{
			configFile = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"ModForge", "userconfig.json");

			Load();
		}

		public UserConfiguration? Current { get; private set; }

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
			var json = JsonSerializer.Serialize(Current, new JsonSerializerOptions
			{
				WriteIndented = true
			});

			Directory.CreateDirectory(Path.GetDirectoryName(configFile)!);
			File.WriteAllText(configFile, json);
		}
	}
}
