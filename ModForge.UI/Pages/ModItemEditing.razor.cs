using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;

namespace ModForge.UI.Pages
{
	public partial class ModItemEditing
	{
		private IModItem? editingModItem;
		private string? icon;

		[Inject]
		public LocalizationService? LocalizationService { get; set; }
		[Inject]
		public NavigationManager? NavigationManager { get; set; }
		[Inject]
		public XmlService? XmlService { get; set; }
		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public ILogger<ModItemEditing>? Logger { get; set; }
		[Inject]
		public IDialogService? DialogService { get; set; }
		[Inject]
		public IconService IconService { get; set; }
		[Parameter]
		public string? Id { get; set; }
		public IModItem? OriginalModItem { get; set; }

		public void ResetModItem()
		{
			if (OriginalModItem is null)
			{
				return;
			}

			editingModItem = OriginalModItem.GetDeepCopy();
			StateHasChanged();
		}

		private void SaveItem()
		{
			if (editingModItem is null || NavigationManager is null)
			{
				Logger?.LogWarning("SaveItem aborted: editingModItem is null.");
				return;
			}

			if (OriginalModItem is null)
			{
				Logger?.LogWarning("SaveItem aborted: originalBuff is null.");
				return;
			}

			if (ModService is null)
			{
				Logger?.LogWarning("SaveItem aborted: ModService is null.");
				return;
			}

			ModService.AddModItem(editingModItem);
			Logger?.LogInformation($"Saved mod buff with Id: {editingModItem.Id}");
			NavigationManager.NavigateTo($"/moditems/{ModService.Mod.Id}");
		}

		private async Task Cancle()
		{
			if (DialogService is null || NavigationManager is null || ModService is null)
			{
				return;
			}

			Logger?.LogInformation("Cancel operation initiated: showing discard confirmation dialog.");

			var parameters = new DialogParameters<TwoButtonExitDialog>()
			{
				{ x => x.ContentText, "Are you sure you want to cancel?" },
				{ x => x.CancelButton, "Cancel" },
				{ x => x.ExitButton, "Discard & Exit" }
			};

			var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<TwoButtonExitDialog>("Return to list", parameters, options);
			var result = await dialog.Result;

			if (result is null)
			{
				return;
			}

			if (!result.Canceled)
			{
				Logger?.LogInformation("User confirmed discard. Navigating back to mod items.");
				NavigationManager.NavigateTo($"/moditems/{ModService.Mod.Id}");
			}
			else
			{
				Logger?.LogInformation("User canceled discard dialog. No navigation performed.");
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (XmlService is null)
				return;

			if (string.IsNullOrEmpty(Id))
				return;

			editingModItem = LoadModItemForEdit(Id);

			if (IconService is null)
			{
				return;
			}

			icon = IconService.GetBase64Icon(editingModItem.Attributes.FirstOrDefault(x => x.Name == "IconId")?.Value.ToString() ?? editingModItem.Attributes.FirstOrDefault(x => x.Name == "icon_id")?.Value.ToString() ?? string.Empty);
			StateHasChanged();
		}

		private IModItem? LoadModItemForEdit(string id)
		{
			OriginalModItem = ModService?.Mod.ModItems.FirstOrDefault(x => x.Id == id);

			if (OriginalModItem is null)
			{
				OriginalModItem = XmlService.GetModItem(Id);
			}

			return OriginalModItem.GetDeepCopy();
		}
	}
}