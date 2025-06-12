using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KCD2.ModForge.UI.Pages
{
	public partial class ModOverview
	{
		private ModDescription mod;

		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		public void ContinueModding()
		{
			NavigationManager.NavigateTo($"/moditems/{mod.ModId}");
		}

		public void ExportMod()
		{
			ModService.ExportMod(mod);
			ModService.WriteModCollectionAsJson();
			Snackbar.Add(
				"Mod successfully created",
				Severity.Success,
				config =>
				{
					config.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
				});
			ModService.ClearCurrentMod();
			NavigationManager.NavigateTo("/");
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			mod = ModService.GetCurrentMod();
			StateHasChanged();
		}
	}
}
