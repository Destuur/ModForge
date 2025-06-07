using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModCollectionComponent
	{
		private ModCollection? mods = new();

		[Inject]
		public ModService ModService { get; set; }

		private void DeleteMod(ModDescription mod)
		{
			if (mod == null) return;

			ModService.RemoveMod(mod);
			mods = ModService.GetAllMods();
			StateHasChanged(); // UI aktualisieren
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			ModService.Load();
			mods = ModService.GetAllMods();
			StateHasChanged();
		}
	}
}
