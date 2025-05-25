using KCD2.XML.Tool.Shared.Mods;
using Microsoft.AspNetCore.Components;

namespace KCD2.XML.Tool.UI.Components.ModCollectionComponents
{
	public partial class ModCollectionComponent
	{
		[Inject]
		public ModCollection? Mods { get; set; }
	}
}
