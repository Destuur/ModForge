using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.STORM;
using ModForge.Shared.Services;

namespace ModForge.UI.Pages
{
	public partial class StormItems
	{
		[Inject]
		private NavigationManager? NavigationManager { get; set; }
		[Inject]
		public IStringLocalizer<StormItems>? Localizer { get; set; }
		[Inject]
		public StormService? StormService { get; set; }

		private void NavigateToItem(string id)
		{
			NavigationManager.NavigateTo($"/test/{id}");
		}

		protected override void OnInitialized()
		{

		}
	}
}