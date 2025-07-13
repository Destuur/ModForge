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
		private IEnumerable<IAttribute> sortedAttributes => editingModItem!.Attributes.OrderBy(attr => attr.Name == "buff_params" ? 1 : 0);
		private List<IAttribute> filteredAttributes = new();

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

			OriginalModItem = XmlService.GetModItem(Id);

			if (OriginalModItem is null)
				return;

			editingModItem = OriginalModItem.GetDeepCopy();
		}
	}
}