using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Pages
{
	public partial class ModOverview
	{
		private ModDescription mod;

		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public XmlAdapterOfT<Perk> XmlAdapter { get; set; }

		public void ExportMod()
		{
			XmlAdapter.WriteModItems(ModService.Mod);
			ModService.Save();
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			mod = ModService.GetMod();
			StateHasChanged();
		}
	}
}
