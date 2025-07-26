using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;

namespace ModForge.UI.Layout
{
	public partial class NavMenu
	{
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }

		private async Task GetHelp()
		{
			var parameters = new DialogParameters<HelpDialog>()
			{
				{ x => x.ContentText, "This section may be helpful in the future. Who knows..." },
				{ x => x.ButtonText, "Stop yanking my pizzle!" },
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
			await DialogService.ShowAsync<HelpDialog>("Error 1403: Help not found!", parameters, options);
		}
	}
}