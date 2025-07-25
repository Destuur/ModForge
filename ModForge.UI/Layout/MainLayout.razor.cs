using Microsoft.AspNetCore.Components;
using ModForge.Shared.Services;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Layout
{
	public partial class MainLayout
	{
		private MudThemeProvider? _mudThemeProvider;
		private bool _isLoaded = false;

		[Inject]
		public XmlService? XmlToJsonService { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }


		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await base.OnInitializedAsync();
				_isLoaded = true;
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
				StateHasChanged();
			}
		}
	}
}
