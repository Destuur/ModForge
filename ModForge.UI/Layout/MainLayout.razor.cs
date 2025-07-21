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
				await Task.Run(() => XmlToJsonService.TryReadJsonFilesWithFallback());
				StateHasChanged();
			}
		}
	}
}
