using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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
		public NavigationService? NavigationService { get; set; }
		[Parameter]
		public Perk? Perk { get; set; }
		[Parameter]
		public string Placeholder { get; set; } = "No localization data available";

		protected override void OnInitialized()
		{
			base.OnInitialized();
			mod = ModService!.GetCurrentMod();
		}

		private async Task EditPerk(MouseEventArgs args)
		{
			if (NavigationService is null)
			{
				return;
			}
			await NavigationService.NavigateToAsync($"editing/perk/{Perk.Id}");
		}
	}
}
