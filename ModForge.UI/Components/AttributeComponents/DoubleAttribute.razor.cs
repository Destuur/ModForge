﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ModForge.Shared.Models.Abstractions;

namespace ModForge.UI.Components.AttributeComponents
{
	public partial class DoubleAttribute
	{
		[Parameter]
		public IAttribute Attribute { get; set; }
		double CurrentValue
		{
			get => (double)Attribute.Value;
			set => Attribute.Value = value;
		}
		[Parameter]
		public EventCallback<string> RemoveAttribute { get; set; }

		private async Task Remove()
		{
			await RemoveAttribute.InvokeAsync(Attribute.Name);
		}

		private string FormatLabel(string raw)
		{
			if (string.IsNullOrWhiteSpace(raw))
				return string.Empty;

			// Unterstriche durch Leerzeichen ersetzen
			string noUnderscores = raw.Replace("_", " ");

			// CamelCase trennen
			string withSpaces = Regex.Replace(noUnderscores, "(?<!^)([A-Z])", " $1");

			// Jeden Wortanfang großschreiben
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(withSpaces.ToLower());
		}
	}
}
