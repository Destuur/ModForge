using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using KCD2.ModForge.UI.Components.ModSettingComponents;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
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
		public XmlAdapterOfT<Perk> XmlAdapter { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		public void ExportMod()
		{
			XmlAdapter.WriteModItems(Mod);
			ModService.Save();
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

			NavigationManager.NavigateTo($"modItems/{Mod.ModId}");
		}
	}
}
