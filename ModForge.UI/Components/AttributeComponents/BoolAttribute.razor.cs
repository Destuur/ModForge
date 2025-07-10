using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text.RegularExpressions;
using ModForge.Shared.Models.Abstractions;

namespace ModForge.UI.Components.AttributeComponents
{
	public partial class BoolAttribute
	{
		[Parameter]
		public IAttribute Attribute { get; set; }
		[Parameter]
		public EventCallback<string> RemoveAttribute { get; set; }


		private bool BoolValue
		{
			get => CurrentValueString?.ToLower() == "true";
			set
			{
				CurrentValueString = value ? "True" : "False";

			}
		}

		string CurrentValueString
		{
			get => (Attribute.Value is bool b && b) ? "True" : "False";
			set => Attribute.Value = string.Equals(value, "True", StringComparison.OrdinalIgnoreCase);
		}

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
