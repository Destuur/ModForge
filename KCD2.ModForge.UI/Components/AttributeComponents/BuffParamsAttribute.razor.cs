using KCD2.ModForge.Shared;
using KCD2.ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Components.AttributeComponents
{
	public partial class BuffParamsAttribute
	{
		[CascadingParameter]
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

		private void RemoveBuffParam(string key)
		{
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
