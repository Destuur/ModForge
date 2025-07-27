using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.STORM;
using ModForge.Shared.Models.STORM.Selectors;

namespace ModForge.UI.Components.StormComponents
{
	public partial class SelectorComponent
	{
		[Parameter]
		public GenericSelector Selector { get; set; }

		private bool IsOperatingSelector()
		{
			return Selector.Name == "or" || Selector.Name == "and" || Selector.Name == "not";
		}
	}
}