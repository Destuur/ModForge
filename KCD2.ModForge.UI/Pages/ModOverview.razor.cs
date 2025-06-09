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
		public ModItemAdapter<Perk> XmlAdapter { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		public void ExportMod()
		{
			XmlAdapter.WriteModItems(ModService.Mod);
			ModService.Save();
			Snackbar.Add(
				"Mod successfully created",
				Severity.Success,
				config =>
				{
					config.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
				});
			NavigationManager.NavigateTo("/");
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			mod = ModService.GetMod();
			StateHasChanged();
		}
	}
}
