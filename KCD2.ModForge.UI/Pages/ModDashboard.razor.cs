using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Pages
{
	public partial class ModDashboard
	{
		private ModCollection modCollection = new();

		[Inject]
		public ModService ModService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			ModService.Load();
			modCollection = ModService.GetAllMods();
			StateHasChanged();
		}
	}
}
