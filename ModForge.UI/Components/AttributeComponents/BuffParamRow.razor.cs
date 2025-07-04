﻿using ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using ModForge.Shared.Models.Enums;

namespace ModForge.UI.Components.AttributeComponents
{
	public partial class BuffParamRow
	{
		[Parameter]
		public BuffParam BuffParam { get; set; }
		[Parameter]
		public EventCallback<string> RemoveBuffParam { get; set; }




		private IEnumerable<string> enumNames = Enumerable.Empty<string>();
		private Dictionary<string, string> enumDisplayNames = new();

		private string currentEnumString;

		private MathOperation CurrentEnumValue
		{
			get => BuffParam.Operation;
			set
			{
				BuffParam.Operation = value;
				currentEnumString = value.ToString();
			}
		}

		private string CurrentEnumString
		{
			get => currentEnumString;
			set
			{
				currentEnumString = value;

				var parsed = (MathOperation)Enum.Parse(typeof(MathOperation), value);
				BuffParam.Operation = parsed;
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();

			var value = BuffParam.Operation;
			if (value != null && value.GetType().IsEnum)
			{
				enumNames = Enum.GetNames(typeof(MathOperation));

				enumDisplayNames = enumNames.ToDictionary(
					name => name,
					name => SplitCamelCase(name)
				);

				currentEnumString = value.ToString();
			}
			StateHasChanged();
		}

		private string SplitCamelCase(string input)
		{
			return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
		}
	}
}
