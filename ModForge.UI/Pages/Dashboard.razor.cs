using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;

namespace ModForge.UI.Pages
{
	public partial class Dashboard
	{
		public ModCollection CreatedMods { get; set; }
		[Inject]
		public ModService ModService { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (ModService is null)
			{
				return;
			}

			CreatedMods = ModService.GetAllMods();
		}
	}
}