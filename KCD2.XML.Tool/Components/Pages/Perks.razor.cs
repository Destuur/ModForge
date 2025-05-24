using KCD2.XML.Tool.Shared.Adapter;
using KCD2.XML.Tool.Shared.Mods;
using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.XML.Tool.Components.Pages
{
	public partial class Perks
	{
		[Inject]
		public IXmlAdapter? Adapter { get; private set; }
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
