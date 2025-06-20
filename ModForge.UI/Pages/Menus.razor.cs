using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using ModForge.UI.Components.MenuComponents;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Pages
{
	public partial class Menus
	{
		private ModCollection createdMods { get; set; }

		public RenderFragment? CustomRender { get; set; }

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

		public RenderFragment CreateComponent(Type type) => builder =>
		{
			builder.OpenComponent(0, type);
			builder.CloseComponent();
		};

		private void GoToDashboard()
		{
			CustomRender = CreateComponent(typeof(DashboardComponent));
		}

		private void GoToSettings()
		{
			CustomRender = CreateComponent(typeof(Settings));
		}

		private void GoToManageMods()
		{
			CustomRender = CreateComponent(typeof(ModManager));
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			if (ModService is null)
			{
				return;
			}

			CustomRender = CreateComponent(typeof(DashboardComponent));

			createdMods = ModService.GetAllMods();

			var culture = new CultureInfo(UserConfigurationService.Current.Language);
			Thread.CurrentThread.CurrentCulture = culture;
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