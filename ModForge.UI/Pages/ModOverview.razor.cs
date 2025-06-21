using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using MudBlazor;

namespace ModForge.UI.Pages
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
			NavigationManager.NavigateTo($"/moditems/{mod.Id}");
		}

		public void ExportMod()
		{
			ModService.ExportMod(mod);
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
			mod = ModService.Mod;
			StateHasChanged();
		}
	}
}
