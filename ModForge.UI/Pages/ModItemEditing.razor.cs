using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ModForge.UI.Pages
{
	public partial class ModItemEditing
	{
		private IModItem? editingModItem;
		private IEnumerable<IAttribute> sortedAttributes => editingModItem.Attributes.OrderBy(x => x.Value.GetType().Name).ToList();
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
		public ILogger<ModItemEditing> Logger { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
		[Parameter]
		public string? Id { get; set; }
		public IModItem OriginalModItem { get; set; }

		public void ResetModItem()
		{
			//editingModItem = Buff.GetDeepCopy(OriginalModItem);
			StateHasChanged();
		}

		private async Task Cancel()
		{
			Logger?.LogInformation("Cancel operation initiated: showing discard confirmation dialog.");

			var parameters = new DialogParameters<ExitDialog>
	{
		{ x => x.ContentText, "Do you really want to discard all changes?" }
	};

			var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<ExitDialog>("Discard Changes", parameters, options);
			var result = await dialog.Result;

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

			var modItem = XmlService.GetModItem(Id);

			if (modItem is null)
				return;

			editingModItem = modItem;
			//OriginalModItem = IModItem.GetDeepCopy(editingModItem);
		}
	}
}