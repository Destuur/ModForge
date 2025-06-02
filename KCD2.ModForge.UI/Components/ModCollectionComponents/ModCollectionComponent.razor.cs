using KCD2.ModForge.Shared.Models.Mods;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModCollectionComponent
	{
		[Parameter]
		public ModCollection? Mods { get; set; }

		private void DeleteMod(ModDescription mod)
		{
			if (mod == null) return;

			Mods.Remove(mod);
			StateHasChanged(); // UI aktualisieren
		}
	}
}
