using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Components.ModManagerComponents
{
	public partial class ModManagerComponent
	{
		private ModCollection? mods = new();

		[Inject]
		public ModService ModService { get; set; }

		private void DeleteExternalMod(ModDescription mod)
		{
			if (mod == null) return;

			ModService.RemoveModFromExternalCollection(mod);
			mods = ModService.GetAllExternalMods();
			StateHasChanged();
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			ModService.ReadExternalModsFromPak();
			mods = ModService.GetAllExternalMods();
			StateHasChanged();
		}
	}
}
