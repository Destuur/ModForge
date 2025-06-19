using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Mods;

namespace ModForge.UI.Components.DashboardComponents
{
	public partial class ModListItems
	{
		[Parameter]
		public ModCollection Mods { get; set; }
	}
}