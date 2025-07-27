using Microsoft.AspNetCore.Components;
using ModForge.Shared.Services;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Layout
{
	public partial class MainLayout
	{
		private MudThemeProvider? _mudThemeProvider;
		private bool isLoaded;
		private bool drawerOpen;


		[Inject]
		public XmlService? XmlToJsonService { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }

		private void ToggleDrawer()
		{
			drawerOpen = !drawerOpen;
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await base.OnInitializedAsync();
				if (XmlToJsonService is null)
				{
					return;
				}
				await Task.Run(() => XmlToJsonService.TryReadXmlFiles());

				var language = UserConfigurationService.Current.Language;
				var culture = string.IsNullOrEmpty(language) ? CultureInfo.CurrentCulture : new CultureInfo(UserConfigurationService.Current.Language);

				CultureInfo.DefaultThreadCurrentCulture = culture;
				CultureInfo.DefaultThreadCurrentUICulture = culture;
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = culture;
				isLoaded = true;
				StateHasChanged();
			}
		}
	}
}
