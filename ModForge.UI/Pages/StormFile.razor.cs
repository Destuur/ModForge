using Microsoft.AspNetCore.Components;
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
		public StormDto? Storm { get; set; }

		protected override void OnInitialized()
		{
			Storm = StormService!.GetStormDto(Id);
		}
	}
}