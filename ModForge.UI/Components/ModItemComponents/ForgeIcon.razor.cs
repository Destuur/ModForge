using Microsoft.AspNetCore.Components;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class ForgeIcon
	{
		[Parameter]
		public bool IsEnabled { get; set; }
		[Parameter]
		public EventCallback ToggleDrawer { get; set; }

		private async Task Toggle()
		{
			await ToggleDrawer.InvokeAsync();
		}
	}
}