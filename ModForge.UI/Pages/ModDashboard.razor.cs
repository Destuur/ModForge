using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Pages
{
	public partial class ModDashboard
	{
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			var culture = new CultureInfo("de"); // oder "en-US", "de-DE", etc.

			// Kultur für Formatierungen
			Thread.CurrentThread.CurrentCulture = culture;

			// UI-Kultur für Ressourcen
			Thread.CurrentThread.CurrentUICulture = culture;

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
					Navigation.NavigateTo("/settings");
				}
			}
		}
	}
}
