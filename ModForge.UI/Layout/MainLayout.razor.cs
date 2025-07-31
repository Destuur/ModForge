using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ModForge.Shared.Services;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Layout
{
	public partial class MainLayout
	{
		private MudThemeProvider? mudThemeProvider;
		private bool isLoaded;
		private bool drawerOpen;

		[Inject]
		public XmlService? XmlToJsonService { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		private void ToggleDrawer()
		{
			drawerOpen = !drawerOpen;
		}

		protected override async Task OnInitializedAsync()
		{
			var language = UserConfigurationService.Current.Language ?? "en";
			var culture = new CultureInfo(language);

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;

			if (XmlToJsonService is not null)
			{
				// Optional: Wenn TryReadXmlFiles IO-intensiv ist, mach es trotzdem asynchron:
				await Task.Run(() => XmlToJsonService.TryReadXmlFiles());
			}

			isLoaded = true;
		}
	}
}
