using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModCollectionComponent
	{
		[Parameter]
		public ModCollection? Mods { get; set; }
		[Inject]
		public ModService ModService { get; set; }

		private void DeleteMod(ModDescription mod)
		{
			if (mod == null) return;

			ModService.RemoveMod(mod);
			Mods = ModService.GetAllMods();
			StateHasChanged(); // UI aktualisieren
		}
	}
}
