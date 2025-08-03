using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text.RegularExpressions;
using ModForge.Shared.Models.Abstractions;

namespace ModForge.UI.Components.AttributeComponents
{
	public partial class StringAttribute
	{
		[Parameter]
		public IAttribute Attribute { get; set; }
		string CurrentValue
		{
			get => (string)Attribute.Value;
			set => Attribute.Value = value;
		}
		[Parameter]
		public EventCallback<string> RemoveAttribute { get; set; }
		[Parameter]
		public EventCallback<string> ResetedValue { get; set; }

		private void ResetValue(string key)
		{
			ResetedValue.InvokeAsync(key);
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
