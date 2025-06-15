using Microsoft.AspNetCore.Components;
using ModForge.Shared.Services;

namespace ModForge.UI.Layout
{
	public partial class NavMenu
	{
		[Inject]
		public NavigationService NavigationService { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		private string GetActiveClass(string targetUri)
		{
			var current = NavigationManager.Uri;
			var baseUri = NavigationManager.BaseUri;
			var relativeCurrent = current.Replace(baseUri, "/");

			return relativeCurrent.StartsWith(targetUri, StringComparison.OrdinalIgnoreCase)
				? "mud-nav-link mud-nav-link-active mud-nav-link-default mud-nav-link-text"
				: "mud-nav-link mud-nav-link-default mud-nav-link-text";
		}
	}
}
