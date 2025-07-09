using ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ModForge.Shared.Models.Enums;

namespace ModForge.UI.Components.AttributeComponents
{
	public partial class BuffParamsAttribute
	{
		[Parameter]
		public IAttribute? Attribute { get; set; }
		IList<BuffParam>? CurrentValues
		{
			get => (IList<BuffParam>)Attribute!.Value;
			set => Attribute!.Value = value;
		}

		private string SplitCamelCase(string input)
		{
			return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
		}

		private string GetTypeLabel()
		{
			return SplitCamelCase(typeof(MathOperation).Name);
		}

		public void AddBuffParam(string key)
		{
			if (CurrentValues is null)
			{
				return;
			}

			CurrentValues.Add(new BuffParam(key, MathOperation.AddAbsolute, 0));
			StateHasChanged();
		}

		private void RemoveBuffParam(string key)
		{
			if (CurrentValues is null)
			{
				return;
			}

			var tempList = new List<BuffParam>();

			foreach (var currentValue in CurrentValues)
			{
				if (currentValue.Key != key)
				{
					tempList.Add(currentValue);
				}
			}

			CurrentValues = tempList;
		}
	}
}
