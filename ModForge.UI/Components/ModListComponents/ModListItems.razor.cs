using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Mods;

namespace ModForge.UI.Components.ModListComponents
{
	public partial class ModListItems
	{
		[Parameter]
		public ModCollection Mods { get; set; }
	}
}