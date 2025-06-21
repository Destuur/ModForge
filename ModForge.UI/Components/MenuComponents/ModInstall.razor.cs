using Microsoft.AspNetCore.Components;
using ModForge.Shared.Services;

namespace ModForge.UI.Components.MenuComponents
{
	public partial class ModInstall
	{
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
	}
}
