using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KCD2.ModForge.UI.Components.BuffComponents
{
	public partial class BuffEditingItem
	{
		private IEnumerable<IAttribute> sortedAttributes => Buff.Attributes.OrderBy(x => (x.Value is IList<BuffParam> ? 1 : 0, x.Value.GetType().Name));

		[Inject]
		public ModService? ModService { get; set; }
		[Parameter]
		public Buff Buff { get; set; }
	}
}
