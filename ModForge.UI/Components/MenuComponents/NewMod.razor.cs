using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModForge.Localizations;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ModForge.UI.Components.MenuComponents
{
	public partial class NewMod
	{
		private string? supportedGameVersion = "0.0.0";
		private List<string> supportedGameVersions = new();
		private bool hasSupportVersion;
		private string name = string.Empty;
		private string description = string.Empty;
		private string author = string.Empty;
		private string version = "1.0";
		private DateTime createdOn = DateTime.Now.Date;
		private string modId = string.Empty;
		private bool modifiesLevel;
		private bool isValid;
		private UserConfigurationService? userConfigurationService;

		[Inject]
		public ModService? ModService { get; set; }
		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
		[Inject]
		public UserConfigurationService? UserConfigurationService
		{
			get => userConfigurationService;
			set
			{
				userConfigurationService = value;
				author = userConfigurationService.Current.UserName;
			}
		}
		[Inject]
		public ILogger<NewMod> Logger { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }

		public bool HasSupportedVersion
		{
			get => hasSupportVersion;
			set
			{
				hasSupportVersion = value;
				if (value == false)
				{
					supportedGameVersions.Clear();
				}
			}
		}

		public void GetModId()
		{
			var modIdStrings = name.Trim().ToLower().Split(' ');
			modId = string.Join('_', modIdStrings);
			Validate();
			StateHasChanged();
		}

		public bool RemoveVersion(string version)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				Logger.LogWarning("RemoveSupportedVersion: version is null or empty.");
				return false;
			}

			bool removed = supportedGameVersions.Remove(version);

			if (removed)
			{
				Logger.LogInformation("Version '{Version}' successfully removed..", version);
			}
			else
			{
				Logger.LogInformation("Version '{Version}' currently not in list.", version);
			}

			return removed;
		}

		public void StartModding()
		{
			if (ModService is null)
			{
				Logger?.LogWarning("ModService is null. Cannot start modding.");
				return;
			}

			if (Navigation is null)
			{
				Logger?.LogWarning("Navigation service is null. Cannot start modding.");
				return;
			}

			var mod = ModService.CreateNewMod(name, description, author, version, createdOn, modId, modifiesLevel, supportedGameVersions);

			if (mod is null)
			{
				Logger?.LogWarning("Current mod is null. Aborting start modding.");
				return;
			}

			mod.ModItems.Clear();
			Navigation.NavigateTo($"moditems/perks/{mod.Id}");
		}

		private async Task Cancel()
		{
			var parameters = new DialogParameters<TwoButtonExitDialog>()
			{
				{ x => x.ContentText, "Are you sure you want to cancel?" },
				{ x => x.CancelButton, "Cancel" },
				{ x => x.ExitButton, "Discard & Exit" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };

			if (DialogService is null)
			{
				Logger?.LogError("DialogService is null. Cannot show discard changes dialog.");
				return;
			}

			var dialog = await DialogService.ShowAsync<TwoButtonExitDialog>("No Epic Mod Today?", parameters, options);
			var result = await dialog.Result;


			if (result.Canceled)
			{
				return;
			}

			await ChangeChildContent.InvokeAsync(typeof(Dashboard));
		}

		public void AddVersion()
		{
			if (string.IsNullOrEmpty(supportedGameVersion))
			{
				supportedGameVersion = string.Empty;
				return;
			}

			if (supportedGameVersion == "0.0.0")
			{
				supportedGameVersion = string.Empty;
				return;
			}

			var regex = new Regex(@"^\d+\.\d+(\.\d+|\*)$");

			if (!regex.IsMatch(supportedGameVersion))
			{
				supportedGameVersion = string.Empty;
				return;
			}

			supportedGameVersions.Add(supportedGameVersion!);
			supportedGameVersion = "0.0.0";
		}

		// TODO: Versionsnummer kann mit 0.0.0134124 eingegeben werden.
		private string ValidateVersion(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return "Version darf nicht leer sein. Erlaubt sind: 1.2.3 oder 1.2*";

			// Erlaubt entweder:
			// - 1.2.3   → drei Zahlen mit Punkten
			// - 1.2*    → zwei Zahlen mit Punkt und danach ein Stern
			var regex = new Regex(@"^\d+\.\d+(\.\d+|\*)$");

			if (!regex.IsMatch(value))
				return "Ungültiges Format. Erlaubt sind: 1.2.3 oder 1.2*";

			return string.Empty;
		}

		private string ValidateModName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return "The mod name can't be empty.";
			}

			// Nur Buchstaben und Leerzeichen erlaubt
			var regex = new Regex(@"^[a-zA-Z\s]+$");

			if (!regex.IsMatch(value))
			{
				return "The mod name may only contain letters and spaces. No numbers or special characters allowed.";
			}

			var modIdStrings = name.Trim().ToLower().Split(' ');
			var tempModId = string.Join('_', modIdStrings);

			if (ModService.ModCollection.FirstOrDefault(x => x.Id == tempModId) is not null)
			{
				return "A mod with this name is already in your collection.";
			}

			return string.Empty;
		}

		private void Validate()
		{
			isValid = !string.IsNullOrEmpty(name) ||
				!string.IsNullOrWhiteSpace(name) && ValidateModName(name) == string.Empty;

			if (ModService.ModCollection.FirstOrDefault(x => x.Id == modId) is not null)
			{
				isValid = false;
			}
		}

		private void SetLanguage()
		{
			var language = UserConfigurationService.Current.Language;
			var culture = string.IsNullOrEmpty(language) ? CultureInfo.CurrentCulture : new CultureInfo(UserConfigurationService.Current.Language);

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		protected override void OnInitialized()
		{
			SetLanguage();
			Validate();
			StateHasChanged();
		}
	}
}