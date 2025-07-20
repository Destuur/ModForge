using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Abstractions;
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
		[Inject]
		public LocalizationService LocalizationService { get; set; }
		[Inject]
		public XmlService XmlService { get; set; }
		[Parameter]
		public string ModId { get; set; }
		public IModItem? SelectedModItem { get; set; }

		private void SelectModItem(IModItem modItem)
		{
			if (modItem is null)
			{
				return;
			}

			SelectedModItem = modItem;
			StateHasChanged();
		}

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

		private void DeleteModItem(string id)
		{
			if (ModService is null || string.IsNullOrEmpty(id))
			{
				return;
			}
			var foundModItem = ModService.Mod.ModItems.FirstOrDefault(x => x.Id == id);

			if (foundModItem is null)
			{
				return;
			}

			ModService.Mod.ModItems.Remove(foundModItem);
			StateHasChanged();
		}

		private void EditModItem(string id)
		{
			if (NavigationManager is null || string.IsNullOrEmpty(id))
			{
				return;
			}

			NavigationManager.NavigateTo($"/editing/moditem/{id}");
		}

		private bool HasDifferenceToOriginalMod(string id, IAttribute attribute)
		{
			var originalModItem = XmlService.GetModItem(id);

			if (originalModItem is null)
			{
				return true;
			}

			var foundAttribute = originalModItem.Attributes.FirstOrDefault(x => x.Name == attribute.Name);

			if (foundAttribute is null)
			{
				return true;
			}

			if (attribute.Name == "perk_name")
			{

			}

			return !foundAttribute.Value.Equals(attribute.Value);
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (ModService.TryGetModFromCollection(ModId))
			{
				mod = ModService.Mod;
			}

			StateHasChanged();
		}
	}
}
