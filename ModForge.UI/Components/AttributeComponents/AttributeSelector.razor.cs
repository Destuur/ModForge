using ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;

namespace ModForge.UI.Components.AttributeComponents
{
	public partial class AttributeSelector
	{
		private BuffParamsAttribute? childComponent;

		[Parameter]
		public IAttribute Attribute { get; set; }
		[Parameter]
		public EventCallback<string> RemoveAttribute { get; set; }

		private void AddBuffParam()
		{
			childComponent.AddBuffParam();
		}

		private async Task Remove(string attribute)
		{
			await RemoveAttribute.InvokeAsync(attribute);
		}
	}
}
