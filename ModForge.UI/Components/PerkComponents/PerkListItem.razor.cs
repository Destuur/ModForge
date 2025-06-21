using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;

namespace ModForge.UI.Components.PerkComponents
{
	public partial class PerkListItem
	{
		private ModDescription? mod;
		private string languageKey = "en";

		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public LocalizationService? LocalizationService { get; set; }
		[Inject]
		public NavigationManager? Navigation { get; set; }
		[Inject]
		public ILogger<PerkListItem>? Logger { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }
		[Parameter]
		public Perk? Perk { get; set; }
		[Parameter]
		public string Placeholder { get; set; } = "No localization data available";

		protected override void OnInitialized()
		{
			base.OnInitialized();
			mod = ModService!.Mod;
			languageKey = UserConfigurationService.Current.Language;
		}

		private void EditPerk(MouseEventArgs args)
		{
			if (Navigation is null)
			{
				Logger?.LogWarning("Navigation service is null, cannot navigate to perk editor.");
				return;
			}

			if (Perk is null || Perk.Id == null)
			{
				Logger?.LogWarning("Perk or Perk.Id is null, cannot navigate.");
				return;
			}

			Navigation.NavigateTo($"editing/perk/{Perk.Id}");
		}
	}
}
