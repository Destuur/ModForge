using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Components.MenuComponents
{
	public partial class Dashboard
	{
		private ModCollection createdMods;
		private ModCollection externalMods;
		private string buttonContent;
		private bool isCreatedVisible = true;

		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }

		private void ToggleModList()
		{
			isCreatedVisible = !isCreatedVisible;
		}

		private async Task OnButtonClicked()
		{
			await ChangeChildContent.InvokeAsync(typeof(NewMod));
		}

		private void RefreshMods()
		{
			createdMods = ModService.ModCollection;
			externalMods = ModService.ExternalModCollection;
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			if (ModService is null)
			{
				return;
			}
			ModService.InitiateModCollections();
			createdMods = ModService.ModCollection;
			externalMods = ModService.ExternalModCollection;

			var culture = new CultureInfo(string.IsNullOrEmpty(UserConfigurationService.Current.Language) ? "en" : UserConfigurationService.Current.Language);
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;

			buttonContent = L["NewModButton"].Value;

			if (string.IsNullOrEmpty(UserConfigurationService.Current.GameDirectory))
			{
				var parameters = new DialogParameters<MoreModItemsDialog>
				{
					{ x => x.ButtonText, "Bring me the sacred data." }
				};

				var options = new DialogOptions() { CloseButton = false, MaxWidth = MaxWidth.ExtraSmall, BackdropClick = false, BackgroundClass = "entry-dialog", CloseOnEscapeKey = false, FullWidth = true };

				var dialog = await DialogService.ShowAsync<EntryDialog>("A Peasant With No Pitchfork entered", parameters, options);
				var result = await dialog.Result;

				if (result.Canceled == false)
				{
					await ChangeChildContent.InvokeAsync(typeof(Settings));
				}
			}
			StateHasChanged();
		}
	}
}