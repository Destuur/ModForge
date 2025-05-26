using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KCD2.ModForge.UI.Components.PerkComponents
{
	public partial class PerkListItem
	{
		private ModDescription? mod;
		private string descAttribute = "perk_ui_desc";
		private string loreDescAttribute = "perk_ui_lore_desc";
		private string nameAttribute = "perk_ui_name";

		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public LocalizationService? LocalizationService { get; set; }
		[Inject]
		public NavigationManager? NavigationManager { get; set; }
		[Parameter]
		public Perk? Perk { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();
			mod = ModService!.GetMod();
		}
		private void EditPerk(MouseEventArgs args)
		{
			NavigationManager.NavigateTo($"editing/perk/{Perk.Id}");
			//NavigationManager.NavigateTo($"");
		}
	}
}
