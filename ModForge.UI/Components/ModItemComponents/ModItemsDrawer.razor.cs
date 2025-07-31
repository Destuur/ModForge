using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModForge.Localizations;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using ModForge.UI.Pages;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class ModItemsDrawer
	{
		[Inject]
		public ModService? ModService { get; private set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		[Inject]
		public LocalizationService LocalizationService { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Parameter]
		public bool IsOpen { get; set; }

		private void ToggleDrawer()
		{
			IsOpen = !IsOpen;
		}

		private void EditModItem(string id)
		{
			if (NavigationManager is null || string.IsNullOrEmpty(id))
			{
				return;
			}

			NavigationManager.NavigateTo($"/editing/moditem/{id}");
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

		private async Task Checkout()
		{
			var parameters = new DialogParameters<MoreModItemsDialog>
			{
				{ x => x.ContentText, "Hast thou pulled enough pizzles? Depart then, and shape thy mod!" },
				{ x => x.ButtonText, "Create Mod" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<MoreModItemsDialog>("Create Your Mod", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled == false)
			{
				NavigationManager.NavigateTo($"/modoverview/{ModService.Mod.Id}");
			}
		}
	}
}