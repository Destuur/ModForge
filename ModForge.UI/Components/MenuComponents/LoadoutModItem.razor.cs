using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Mods;

namespace ModForge.UI.Components.MenuComponents
{
	public partial class LoadoutModItem
	{
		[Parameter]
		public ModDescription Mod { get; set; }
		[Parameter]
		public bool IsHovered { get; set; }
	}
}