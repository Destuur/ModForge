using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModListItem
	{
		[Parameter]
		public ModDescription? Mod { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Parameter]
		public EventCallback<ModDescription> OnDelete { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		public void ExportMod()
		{
			ModService.ExportMod(Mod);
			ModService.WriteModCollectionAsJson();
			Snackbar.Add(
				"Mod successfully created",
				Severity.Success,
				config =>
				{
					config.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
				});
		}

		public void EditMod()
		{
			if (ModService is null)
			{
				return;
			}

			ModService.SetCurrentMod(Mod);
			NavigationManager.NavigateTo($"/modoverview");
		}

		public void DeleteMod()
		{
			OnDelete.InvokeAsync(Mod);
		}
	}
}
