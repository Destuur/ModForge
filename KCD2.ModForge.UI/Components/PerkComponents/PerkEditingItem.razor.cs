using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KCD2.ModForge.UI.Components.PerkComponents
{
	public partial class PerkEditingItem
	{
		private IEnumerable<IAttribute> sortedAttributes => EditingPerk.Attributes.OrderBy(x => x.Value.GetType().Name);

		[Inject]
		public ModService? ModService { get; set; }
		[Parameter]
		public Perk EditingPerk { get; set; }
		[Parameter]
		public Perk OriginalPerk { get; set; }
	}
}
