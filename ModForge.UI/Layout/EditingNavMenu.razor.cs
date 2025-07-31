using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModForge.Localizations;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;

namespace ModForge.UI.Layout
{
	public partial class EditingNavMenu
	{
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }

		[Inject]
		public IconService IconService { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public ILogger<EditingNavMenu> Logger { get; set; }
		[Parameter]
		public EventCallback ToggledDrawer { get; set; }


		private async Task ExitModding()
		{
			if (string.IsNullOrEmpty(ModService.Mod.Id))
			{
				NavigationManager.NavigateTo($"/");
				return;
			}
			if (ModService.Mod.ModItems.Count == 0)
			{
				await ExecuteTwoButtonExitDialog();
			}
			else
			{
				await ExecuteThreeButtonExitDialog();
			}
		}

		private async Task ExecuteThreeButtonExitDialog()
		{
			var parameters = new DialogParameters<ExitDialog>
			{
				{ x => x.ContentText, "Do you really want to cancel your current modding process? Any unsaved changes will be lost." },
				{ x => x.CancelButton, "Continue Modding" },
				{ x => x.ExitButton, "Discard & Exit" },
				{ x => x.SaveAndExitButton, "Save Changes & Exit" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };

			var dialog = await DialogService.ShowAsync<ExitDialog>("Cancel Modding", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled)
			{
				return;
			}

			if ((bool)(result.Data ?? false))
			{
				SaveMod();
			}

			ModService.ClearCurrentMod();
			NavigationManager.NavigateTo($"/");
		}

		public void SaveMod()
		{
			Snackbar.Add(
				"Mod successfully saved",
				Severity.Success,
				config =>
				{
					config.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
				});
			ModService.ExportMod(ModService.Mod);
			ModService.ClearCurrentMod();
		}

		private async Task ExecuteTwoButtonExitDialog()
		{
			var parameters = new DialogParameters<TwoButtonExitDialog>()
			{
				{ x => x.ContentText, "Are you sure you want to cancel your current modding process?" },
				{ x => x.CancelButton, "Cancel" },
				{ x => x.ExitButton, "Discard & Exit" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };

			if (DialogService is null)
			{
				Logger?.LogError("DialogService is null. Cannot show discard changes dialog.");
				return;
			}

			var dialog = await DialogService.ShowAsync<TwoButtonExitDialog>("No Epic Mod Today?", parameters, options);
			var result = await dialog.Result;


			if (result.Canceled)
			{
				return;
			}

			NavigationManager.NavigateTo($"/");
		}

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

		private void ToggleDrawer()
		{
			ToggledDrawer.InvokeAsync();
		}
	}
}