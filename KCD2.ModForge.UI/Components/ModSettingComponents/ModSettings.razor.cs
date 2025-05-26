using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace KCD2.ModForge.UI.Components.ModSettingComponents
{
	public partial class ModSettings
	{
		private ModSettingForm? modSettingForm;
		private ModSettingIconPicker? modSettingIconPicker;

		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public NavigationManager? Navigation { get; set; }

		public async Task SaveMod()
		{
			await modSettingIconPicker!.SaveMod();
			await modSettingForm!.SaveMod();
		}

		public async Task GenerateMod()
		{
			if (ModService is null)
			{
				return;
			}

			await modSettingIconPicker!.SaveMod();
			await modSettingForm!.SaveMod();

			var mod = await ModService.GenerateMod();

			Navigation!.NavigateTo($"perks/{mod.ModId}");
		}
	}
}
