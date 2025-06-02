using KCD2.ModForge.Shared.Models.Mods;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModListItem
	{
		[Parameter]
		public ModDescription? Mod { get; set; }

		public string RandomText()
		{
			return "Scheiße geil!";
		}
	}
}
