using KCD2.ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Components.AttributeComponents
{
	public partial class EnumAttribute
	{
		[CascadingParameter]
		public IAttribute Attribute { get; set; }

		private Type enumType;
		private IEnumerable<string> enumNames = Enumerable.Empty<string>();
		private Dictionary<string, string> enumDisplayNames = new();

		private string currentEnumString;

		// Strongly typed Access to Enum Value
		private Enum CurrentEnumValue
		{
			get => Attribute.Value as Enum;
			set
			{
				Attribute.Value = value;
				currentEnumString = value.ToString();
			}
		}

		// Used by MudSelect
		private string CurrentEnumString
		{
			get => currentEnumString;
			set
			{
				currentEnumString = value;

				if (enumType != null)
				{
					var parsed = (Enum)Enum.Parse(enumType, value);
					Attribute.Value = parsed;
				}
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();

			var value = Attribute.Value;
			if (value != null && value.GetType().IsEnum)
			{
				enumType = value.GetType();
				enumNames = Enum.GetNames(enumType);

				enumDisplayNames = enumNames.ToDictionary(
					name => name,
					name => SplitCamelCase(name)
				);

				currentEnumString = value.ToString();
			}
		}

		private string SplitCamelCase(string input)
		{
			return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
		}

		private string GetTypeLabel()
		{
			return enumType != null ? SplitCamelCase(enumType.Name) : string.Empty;
		}

	}

}
