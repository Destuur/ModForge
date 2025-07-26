using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Models.STORM;
using ModForge.Shared.Services;

namespace ModForge.UI.Pages
{
	public partial class StormFile
	{
		[Parameter]
		public string? Id { get; set; }
		[Inject]
		public StormService? StormService { get; set; }
		[Inject]
		public NavigationManager? NavigationManager { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		public StormDto? Storm { get; set; }

		protected override void OnInitialized()
		{
			Storm = StormService!.GetStormDto(Id);
		}
	}
}