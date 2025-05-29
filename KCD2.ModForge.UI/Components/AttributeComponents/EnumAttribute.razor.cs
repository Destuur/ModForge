using KCD2.ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Components.AttributeComponents
{
	public partial class EnumAttribute
	{
		object currentEnumValue;

		[CascadingParameter]
		public IAttribute Attribute { get; set; }


		IEnumerable<string> enumNames = Enumerable.Empty<string>();

		protected override void OnParametersSet()
		{
			if (currentEnumValue != null && currentEnumValue.GetType().IsEnum)
			{
				enumNames = Enum.GetNames(currentEnumValue.GetType());
			}
			else
			{
				enumNames = Enumerable.Empty<string>();
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			currentEnumValue = Attribute.Value;
		}
	}
}
