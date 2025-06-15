using ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ModForge.UI.Layout
{
	public partial class MainLayout
	{
		private MudThemeProvider? _mudThemeProvider;
		private bool _isLoaded = false;

		[Inject]
		public XmlToJsonService? XmlToJsonService { get; set; }
		[Inject]
		public NavigationService? NavigationService { get; set; }
		public string ModName { get; set; }

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_isLoaded = true;
				StateHasChanged();
				await base.OnInitializedAsync();
				if (XmlToJsonService is null)
				{
					return;
				}
				XmlToJsonService.TryReadJsonFiles();
			}
		}
	}
}
