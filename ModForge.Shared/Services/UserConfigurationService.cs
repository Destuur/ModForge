using Microsoft.Extensions.Logging;
using ModForge.Shared.Models.User;
using System.Text.Json;

namespace ModForge.Shared.Services
{
	public class UserConfigurationService
	{
		private readonly string configFile;
		private readonly ILogger<UserConfigurationService> logger;
		private JsonSerializerOptions jsonSettings = new JsonSerializerOptions()
		{
			WriteIndented = true
		};

		public UserConfigurationService(ILogger<UserConfigurationService> logger)
		{
			this.logger = logger;

			configFile = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"ModForge", "userconfig.json");

			Load();
		}

		public UserConfiguration? Current { get; set; }

		private void Load()
		{
			try
			{
				if (File.Exists(configFile))
				{
					var json = File.ReadAllText(configFile);
					Current = JsonSerializer.Deserialize<UserConfiguration>(json, jsonSettings)
							  ?? new UserConfiguration();
					logger.LogInformation("User configuration loaded successfully.");
				}
				else
				{
					Current = new UserConfiguration();
					logger.LogInformation("Config file not found. Initialized new UserConfiguration.");
				}
			}
			catch (JsonException jex)
			{
				logger.LogError(jex, "Failed to deserialize the user configuration. Using default configuration.");
				Current = new UserConfiguration();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Unexpected error while loading user configuration. Using default configuration.");
				Current = new UserConfiguration();
			}
		}

		public void Save()
		{
			try
			{
				var directory = Path.GetDirectoryName(configFile);
				if (!string.IsNullOrEmpty(directory))
				{
					Directory.CreateDirectory(directory);
				}

				var json = JsonSerializer.Serialize(Current, jsonSettings);
				File.WriteAllText(configFile, json);
				logger.LogInformation("User configuration saved successfully.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to save user configuration.");
			}
		}
	}
}
