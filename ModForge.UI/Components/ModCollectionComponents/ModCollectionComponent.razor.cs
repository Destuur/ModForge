using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModCollectionComponent
	{
		private ModCollection? mods = new();

		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }

		private async Task DeleteAllMods()
		{
			var parameters = new DialogParameters<MoreModItemsDialog>
			{
				{ x => x.ContentText, "Do you really want to delete all mod entries? There is no turning back from this point!" },
				{ x => x.ButtonText, "Be gone, mods!" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<MoreModItemsDialog>("Nuke Mods", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled == false)
			{
				ModService.ClearModCollection();
			}
		}

		private void DeleteMod(ModDescription mod)
		{
			if (mod == null) return;

			ModService.RemoveModFromCollection(mod);
			mods = ModService.GetAllMods();
			StateHasChanged(); // UI aktualisieren
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			ModService.ReadModCollectionFromJson();
			mods = ModService.GetAllMods();
			StateHasChanged();
		}
	}
}
