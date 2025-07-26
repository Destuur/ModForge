using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.STORM;

namespace ModForge.UI.Components.StormComponents
{
	public partial class StormItem
	{
		[Parameter]
		public StormDto Storm { get; set; }
	}
}