using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ModForge.Localizations;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Services;
using ModForge.UI.Components.MenuComponents;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class Consumables
	{
		private List<IModItem> consumables;
		private int selectedRowNumber = -1;
		private bool isLoaded = false;
		private MudMenu contextMenu;
		private IModItem? contextRow;
		private bool rightClick = true;
		private bool isOpen;

		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Parameter]
		public string ModId { get; set; }
		[Parameter]
		public EventCallback ToggledDrawer { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public ILogger<Loadouts> Logger { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public XmlService XmlService { get; set; }
		[Inject]
		public LocalizationService LocalizationService { get; set; }
		[Inject]
		public IJSRuntime JSRuntime { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		public string SearchText { get; set; }
		public IModItem? SelectedModItem { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }


		private Func<IModItem, int, string> SelectRowClass => (modItem, row) =>
		{
			if (selectedRowNumber == row)
			{
				selectedRowNumber = -1;
				return string.Empty;
			}
			else if (modItem != null && modItem.Equals(SelectedModItem))
			{
				selectedRowNumber = row;
				return "selected";
			}
			else
			{
				return string.Empty;
			}
		};
		private void SetLanguage()
		{
			var language = UserConfigurationService.Current.Language;
			var culture = string.IsNullOrEmpty(language) ? CultureInfo.CurrentCulture : new CultureInfo(UserConfigurationService.Current.Language);

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		private void SelectModItem(IModItem modItem)
		{
			if (modItem is null)
			{
				return;
			}

			SelectedModItem = modItem;
			StateHasChanged();
		}

		private async Task OpenMenuContent(DataGridRowClickEventArgs<IModItem> args)
		{
			contextRow = args.Item;
			await contextMenu.OpenMenuAsync(args.MouseEventArgs);
		}

		public async Task ToggleDrawer()
		{
			isOpen = !isOpen;
		}

		double GetAttributeAsDouble(IModItem item, string name)
		{
			var attr = item.Attributes?.FirstOrDefault(a => a?.Name?.ToLower() == name.ToLower());
			return Convert.ToDouble(attr?.Value ?? 0);
		}

		string GetAttributeAsString(IModItem item, string name)
		{
			var attr = item.Attributes?.FirstOrDefault(a => a?.Name?.ToLower() == name.ToLower());
			return attr?.Value?.ToString() ?? string.Empty;
		}


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

		private Func<IModItem, bool> ModItemSearch => item =>
		{
			if (string.IsNullOrWhiteSpace(SearchText))
				return true;

			var search = SearchText.ToLower();

			var localized = LocalizationService.GetName(item);
			if (!string.IsNullOrEmpty(localized) && localized.ToLower().Contains(search))
				return true;

			var nameAttr = item.Attributes?.FirstOrDefault(attr => attr?.Name?.ToLower().Contains("name") == true);
			var nameValue = nameAttr?.Value?.ToString();

			if (!string.IsNullOrEmpty(nameValue) && nameValue.ToLower().Contains(search))
				return true;

			return false;
		};


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

			var name = LocalizationService.GetDescription(modItem);

			if (name is null)
			{
				return "Description not found";
			}

			return name;
		}

		protected override async Task OnInitializedAsync()
		{
			SetLanguage();
			ModService.TryGetModFromCollection(ModId);
			consumables = await Task.Run(() => XmlService.Consumeables.ToList());
			isLoaded = true;
		}
	}
}