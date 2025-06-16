using ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ModForge.UI.Pages
{
	public partial class Settings
	{
		private string name;
		private string gameDirectory;
		private bool isLoading;

		[Inject]
		public IFolderPickerService? FolderPickerService { get; set; }
		[Inject]
		public UserConfigurationService? UserConfigurationService { get; set; }
		[Inject]
		public XmlToJsonService? XmlToJsonService { get; set; }
		[Inject]
		public ISnackbar SnackBar { get; set; }
		[Inject]
		public NavigationManager Navigation { get; set; }

		public void Save()
		{
			UserConfigurationService.Current.UserName = name;
			UserConfigurationService.Current.GameDirectory = gameDirectory;
			UserConfigurationService.Save();

			SnackBar.Add(
				"Settings Saved! Now - IMPORT THE DATA",
				Severity.Success,
				config =>
				{
					config.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
				});
		}

		private string ValidateName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return "A nameless modder? Unacceptable.";

			if (name.Trim().ToLower() == "henry")
				return "You can't be Henry. He's already busy getting hungry.";

			if (name.Trim().ToLower() == "hans")
				return "Alas, 'Hans' is a common name indeed — but try something less... peasant-like, if you please.";

			if (name.Trim().ToLower() == "hanush")
				return "Master Hanush forbids the use of this name here. Choose wisely, lest you anger the scholar.";

			if (name.Trim().ToLower().Contains("radzig"))
				return "Radzig’s shadow looms large; only one Radzig may grace this realm. Pick another name, brave one.";

			if (name.Trim().ToLower() == "brabant")
				return "Brabant’s lands are already claimed. Use a different name unless you seek a feud.";

			if (name.Trim().ToLower() == "divish")
				return "Diwish may be the loyal husband, but some whisper that the true son of Lady Stefanie is a certain young Henry’s bastard. Best pick another name before rumors follow you like a shadow.";

			return null; // valid
		}

		public void BackToDashboard()
		{
			Navigation.NavigateTo("/");
		}

		public void ImportGameData()
		{
			isLoading = true;
			if (XmlToJsonService is null)
			{
				return;
			}
			try
			{
				XmlToJsonService.ConvertXmlToJsonAsync();
			}
			finally
			{
				isLoading = false;
			}

			SnackBar.Add(
				"Data imported successfully! The anvil is hot — time to start hammering those mods into shape!",
				Severity.Success,
				config =>
				{
					config.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
				});
			Navigation.NavigateTo("/");
		}

		private async Task SelectGameDirectory()
		{
			if (FolderPickerService is null)
			{
				return;
			}
			if (UserConfigurationService is null)
			{
				return;
			}

			var selected = await FolderPickerService.PickFolderAsync();

			if (string.IsNullOrEmpty(selected))
			{
				return;
			}

			if (ValidatePath(selected))
			{
				gameDirectory = selected;
			}
			else
			{
				SnackBar.Add(
				"No valid Game Directory",
				Severity.Error,
				config =>
				{
					config.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
				});
			}
		}

		public bool ValidatePath(string basePath)
		{
			if (!Directory.Exists(basePath))
			{
				Console.WriteLine("Path does not exist!");
				return false;
			}

			string dataPath = Path.Combine(basePath, "Data");
			string localizationPath = Path.Combine(basePath, "Localization");
			string tablesPakPath = Path.Combine(dataPath, "Tables.pak");

			bool hasDataFolder = Directory.Exists(dataPath);
			bool hasLocalizationFolder = Directory.Exists(localizationPath);
			bool hasTablesPak = File.Exists(tablesPakPath);

			if (!hasDataFolder)
				Console.WriteLine("Ordner 'Data' fehlt.");

			if (!hasLocalizationFolder)
				Console.WriteLine("Ordner 'Localization' fehlt.");

			if (!hasTablesPak)
				Console.WriteLine("Datei 'Tables.pak' fehlt im Ordner 'Data'.");

			return hasDataFolder && hasLocalizationFolder && hasTablesPak;
		}

		private async Task SelectNexusModsDirectory()
		{
			if (FolderPickerService is null)
			{
				return;
			}
			if (UserConfigurationService is null)
			{
				return;
			}

			var selected = await FolderPickerService.PickFolderAsync();
			if (!string.IsNullOrWhiteSpace(selected))
			{
				UserConfigurationService.Current!.NexusModsDirectory = selected;
			}
		}

		public void ParseXmlFiles()
		{
			if (XmlToJsonService is null)
			{
				return;
			}
			XmlToJsonService.ConvertXmlToJsonAsync();
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			name = UserConfigurationService.Current.UserName ?? string.Empty;
			gameDirectory = UserConfigurationService.Current.GameDirectory ?? string.Empty;
		}
	}
}
