using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Pages
{
	public partial class Perks
	{
		[Inject]
		public ModService? ModService { get; private set; }
		[Parameter]
		public string? ModId { get; set; }

		protected override async Task OnInitializedAsync()
		{
			//await Adapter!.Initialize();
			//PerkItems = await Adapter.GetModItems();
			await ModService.SetMod(ModId);
			await base.OnInitializedAsync();
		}
	}
}
