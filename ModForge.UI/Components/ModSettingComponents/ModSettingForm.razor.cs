using Microsoft.AspNetCore.Components;
using ModForge.Shared.Services;
using System.Text.RegularExpressions;

namespace ModForge.UI.Components.ModSettingComponents
{
	public partial class ModSettingForm
	{
		private string name = string.Empty;
		private string description = string.Empty;
		private string author = string.Empty;
		private string version = "1.0";
		private DateTime createdOn = DateTime.Now.Date;
		private string modId = string.Empty;
		private bool modifiesLevel;

		private SupportedGameVersionRow? supportedGameVersionRow;
		private UserConfigurationService? userConfigurationService;

		public ModSettingForm()
		{
			var currentTime = DateTime.Now;
			createdOn = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
		}

		[Inject]
		public ModService? ModService { get; set; }
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
		[Parameter]
		public EventCallback<bool> OnValidityChanged { get; set; }

		private void Validate()
		{
			bool isValid = !string.IsNullOrEmpty(name) ||
				!string.IsNullOrWhiteSpace(name) && ValidateModName(name) == string.Empty;

			if (ModService.GetAllMods().FirstOrDefault(x => x.ModId == modId) is not null)
			{
				isValid = false;
			}

			OnValidityChanged.InvokeAsync(isValid);
		}

		public void GetModId()
		{
			var modIdStrings = name.Trim().ToLower().Split(' ');
			modId = string.Join('_', modIdStrings);
			Validate();
			StateHasChanged();
		}

		public void SaveMod()
		{
			if (string.IsNullOrEmpty(name) ||
				string.IsNullOrEmpty(modId) ||
				ModService is null)
			{
				return;
			}

			if (supportedGameVersionRow is null)
			{
				return;
			}

			supportedGameVersionRow.SaveMod();
			ModService.CreateNewMod(name, description, author, version, createdOn, modId, modifiesLevel);
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

			if (ModService.GetAllMods().FirstOrDefault(x => x.ModId == tempModId) is not null)
			{
				return "A mod with this name is already in your collection.";
			}

			return string.Empty;
		}

		protected override void OnInitialized()
		{
			Validate();
		}
	}
}
