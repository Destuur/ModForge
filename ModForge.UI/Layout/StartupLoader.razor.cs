using Microsoft.JSInterop;

namespace ModForge.UI.Layout
{
	public partial class StartupLoader
	{
		private bool _hidden = false;
		private string _loadingText = "Starte Anwendung...";

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_hidden = true;
				await JS.InvokeVoidAsync("removeInitialLoader");
				StateHasChanged();
			}
		}
		private string GetOverlayClass()
		{
			return _hidden ? "fade-overlay fade-out" : "fade-overlay";
		}

		private string GetCardClass()
		{
			return _hidden ? "p-6 fade-card fade-out" : "p-6 fade-card";
		}

		private string GetTextClass()
		{
			return _hidden ? "loading-text fade-out" : "loading-text";
		}

	}
}