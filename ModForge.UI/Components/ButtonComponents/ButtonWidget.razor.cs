using Microsoft.AspNetCore.Components;

namespace ModForge.UI.Components.ButtonComponents
{
	public partial class ButtonWidget
	{
		[Parameter]
		public string Title { get; set; }
		[Parameter]
		public string Color { get; set; }
	}
}