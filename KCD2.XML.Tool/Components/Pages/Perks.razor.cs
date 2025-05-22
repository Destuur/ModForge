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
		public IEnumerable<IModItem>? PerkItems { get; private set; }
		[Inject]
		public ModService? Service { get; private set; }

		public void AddModItem(IModItem item)
		{
			Service!.AddItem(item);
		}

		protected override async Task OnInitializedAsync()
		{
			await Adapter!.Initialize();
			PerkItems = await Adapter.GetModItems();
			await base.OnInitializedAsync();
		}
	}
}
