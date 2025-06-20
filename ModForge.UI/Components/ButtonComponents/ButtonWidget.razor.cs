using Microsoft.AspNetCore.Components;

namespace ModForge.UI.Components.ButtonComponents
{
	public partial class ButtonWidget
	{
		[Parameter]
		public EventCallback ButtonClicked { get; set; }
		[Parameter]
		public string Title { get; set; }
		[Parameter]
		public string Icon { get; set; }

		private void ButtonClick()
		{
			ButtonClicked.InvokeAsync();
		}
	}
}