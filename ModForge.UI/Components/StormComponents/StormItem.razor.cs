using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Models.STORM;

namespace ModForge.UI.Components.StormComponents
{
	public partial class StormItem
	{
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Parameter]
		public StormDto Storm { get; set; }
	}
}