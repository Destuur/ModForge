using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using ModForge.Localizations;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Services;
using MudBlazor;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class ModItemDetailsView
	{
		[Parameter]
		public IModItem? SelectedModItem { get; set; }
		[Parameter]
		public string? ConsoleCommand { get; set; }
		[Inject]
		public IconService? IconService { get; set; }
		[Inject]
		public IJSRuntime? JSRuntime { get; set; }
		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public ISnackbar? Snackbar { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public XmlService? XmlService { get; set; }
		[Inject]
		public LocalizationService? LocalizationService { get; set; }
		[Inject]
		public NavigationManager? NavigationManager { get; set; }

		private async Task CopyTextToClipboard(string text)
		{
			await JSRuntime.InvokeVoidAsync("clipboardCopy.copyText", text);
			Snackbar.Add("Content copied to clipboard", Severity.Success);
		}

		private void DuplicateModItem(IModItem modItem)
		{
			if (modItem is null || XmlService is null)
			{
				return;
			}
			var newModItem = modItem.GetDeepCopy();
			newModItem.Id = Guid.NewGuid().ToString();
			newModItem.Attributes.FirstOrDefault(x => x.Name.ToLower().Contains("name"))!.Value = $"{LocalizationService.GetName(modItem)} (Copy)";
			newModItem.Attributes.FirstOrDefault(x => x.Name == newModItem.IdKey)!.Value = newModItem.Id;
			if (ModService is null)
			{
				return;
			}
			ModService.AddModItem(newModItem);
			Snackbar.Add("Perk duplicated successfully!", Severity.Success);
			NavigateToModItem(newModItem);
			StateHasChanged();
		}

		public void NavigateToModItem(IModItem modItem)
		{
			if (NavigationManager is null)
			{
				return;
			}
			NavigationManager.NavigateTo($"editing/moditem/{modItem.Id}");
		}

		private string GetName(IModItem modItem)
		{
			if (LocalizationService is null || modItem is null)
			{
				return string.Empty;
			}

			var name = LocalizationService.GetName(modItem);

			if (name is null)
			{
				return "Name not found";
			}

			return name;
		}

		private string GetLoreDescription(IModItem modItem)
		{
			if (LocalizationService is null || modItem is null)
			{
				return string.Empty;
			}

			var name = LocalizationService.GetLoreDescription(modItem);

			if (name is null)
			{
				return "Lore Description not found";
			}

			return name;
		}

		private string GetDescription(IModItem modItem)
		{
			if (LocalizationService is null || modItem is null)
			{
				return string.Empty;
			}

			var description = LocalizationService.GetDescription(modItem);

			if (description is null)
			{
				return "Description not found";
			}

			return description;
		}
	}
}