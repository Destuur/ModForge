using KCD2.ModForge.Shared.Mods;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModCollectionComponent
	{
		[Inject]
		public ModCollection? Mods { get; set; }
	}
}
