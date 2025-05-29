using KCD2.ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Components.AttributeComponents
{
	public partial class AttributeSelector
	{
		[CascadingParameter]
		public IAttribute Attribute { get; set; }
	}
}
