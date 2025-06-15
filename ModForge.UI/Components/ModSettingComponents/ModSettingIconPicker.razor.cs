using Microsoft.AspNetCore.Components;
using ModForge.Shared.Services;

namespace ModForge.UI.Components.ModSettingComponents
{
	public partial class ModSettingIconPicker
	{
		private string path = string.Empty;

		[Inject]
		public ModService? ModService { get; set; }

		public async Task SaveMod()
		{
			await Task.Yield();

			if (ModService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(path))
			{
				ModService.AddModIcon("images/Icons/crime_investigating.png");
			}

			await ModService.AddModIcon(path);
		}
	}
}
