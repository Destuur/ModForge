using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using Microsoft.AspNetCore.Components;
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
		public NavigationService? NavigationService { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }



		private void UpdateChildValidity(bool valid)
		{
			isChildValid = valid;
		}

		public async Task StartModding()
		{
			if (ModService is null)
			{
				return;
			}
			if (NavigationService is null)
			{
				return;
			}

			//modSettingIconPicker!.SaveMod();
			modSettingForm!.SaveMod();
			var mod = ModService.GetCurrentMod();

			if (mod is null)
			{
				return;
			}

			mod.ModItems.Clear();
			await NavigationService.NavigateToAsync($"modItems/{mod.ModId}");
		}

		private async Task Cancel()
		{
			var parameters = new DialogParameters<ChangesDetectedDialog>()
			{
				{ x => x.ContentText, "Do you really want to discard all changes?" },
				{ x => x.ButtonText, "Discard" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<ChangesDetectedDialog>("Discard Changes", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled == false)
			{
				await NavigationService!.NavigateToAsync("/");
			}
		}
	}
}
