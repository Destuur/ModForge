using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Services;
using ModForge.UI.Components.BuffComponents;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;

namespace ModForge.UI.Components.ModSettingComponents
{
	public partial class ModSettings
	{
		private ModSettingForm? modSettingForm;
		private ModSettingIconPicker? modSettingIconPicker;
		private bool isChildValid = false;

		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public ILogger<BuffListItem> Logger { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }


		private void UpdateChildValidity(bool valid)
		{
			isChildValid = valid;
		}

		public void StartModding()
		{
			if (ModService is null)
			{
				Logger?.LogWarning("ModService is null. Cannot start modding.");
				return;
			}

			if (Navigation is null)
			{
				Logger?.LogWarning("Navigation service is null. Cannot start modding.");
				return;
			}

			modSettingForm?.SaveMod();

			var mod = ModService.GetCurrentMod();
			if (mod is null)
			{
				Logger?.LogWarning("Current mod is null. Aborting start modding.");
				return;
			}

			mod.ModItems.Clear();
			Navigation.NavigateTo($"modItems/{mod.ModId}");
		}

		private async Task Cancel()
		{
			var parameters = new DialogParameters<ChangesDetectedDialog>()
			{
				{ x => x.ContentText, "Do you really want to discard all changes?" },
				{ x => x.ButtonText, "Discard" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			if (DialogService is null)
			{
				Logger?.LogError("DialogService is null. Cannot show discard changes dialog.");
				return;
			}

			var dialog = await DialogService.ShowAsync<ChangesDetectedDialog>("Discard Changes", parameters, options);
			var result = await dialog.Result;

			if (!result.Canceled)
			{
				if (Navigation is null)
				{
					Logger?.LogWarning("Navigation service is null. Cannot navigate after cancel.");
					return;
				}
				Navigation.NavigateTo("/");
			}
		}

	}
}
