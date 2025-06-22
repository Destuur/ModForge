using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ModForge.UI.Components.ButtonComponents
{
	public partial class ButtonWidgetNoIcon
	{
		[Parameter]
		public EventCallback ButtonClicked { get; set; }
		[Parameter]
		public string Title { get; set; }
		[Parameter]
		public Typo Typo { get; set; }
		[Parameter]
		public string Background { get; set; } = "var(--button-background-primary)";
		[Parameter]
		public string Width { get; set; } = "100%";

		private void ButtonClick()
		{
			ButtonClicked.InvokeAsync();
		}
	}
}