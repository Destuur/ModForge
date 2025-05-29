using KCD2.ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Components.AttributeComponents
{
	public partial class BoolAttribute
	{
		[CascadingParameter]
		public IAttribute Attribute { get; set; }
		bool CurrentValue
		{
			get => Attribute.Value is bool b && b;
			set => Attribute.Value = value;
		}
	}
}
