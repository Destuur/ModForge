using Microsoft.AspNetCore.Components;

namespace ModForge.UI.Components.ModListComponents
{
	public partial class ModListWidget
	{
		[Parameter]
		public string FirstTitle { get; set; }
		[Parameter]
		public string SecondTitle { get; set; }
		[Parameter]
		public bool FirstTitleActive { get; set; }
		[Parameter]
		public RenderFragment ChildContent { get; set; }
		[Parameter]
		public EventCallback ToggledModList { get; set; }

		private async Task ToggleModList()
		{
			await ToggledModList.InvokeAsync();
		}
	}
}