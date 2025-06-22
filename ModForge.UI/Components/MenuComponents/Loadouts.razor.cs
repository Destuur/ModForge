using Microsoft.AspNetCore.Components;

namespace ModForge.UI.Components.MenuComponents
{
	public partial class Loadouts
	{
		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
	}
}