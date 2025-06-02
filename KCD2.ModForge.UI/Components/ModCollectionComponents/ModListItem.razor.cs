using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModListItem
	{
		[Parameter]
		public ModDescription? Mod { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Parameter]
		public EventCallback<ModDescription> OnDelete { get; set; }
	}
}
