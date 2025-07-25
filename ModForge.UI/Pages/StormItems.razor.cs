using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Shared.Models.STORM;
using ModForge.Shared.Services;

namespace ModForge.UI.Pages
{
	public partial class StormItems
	{
		private IEnumerable<Storm>? stormItems;

		[Inject]
		private NavigationManager? NavigationManager { get; set; }
		[Inject]
		public IStringLocalizer<StormItems>? Localizer { get; set; }
		[Inject]
		public XmlService? XmlService { get; set; }

		protected override void OnInitialized()
		{
			stormItems = XmlService?.StormItems;
		}
	}
}