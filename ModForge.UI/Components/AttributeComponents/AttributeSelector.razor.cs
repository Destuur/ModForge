using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Attributes;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ModForge.UI.Components.AttributeComponents
{
	public partial class AttributeSelector
	{
		private BuffParamsAttribute? childComponent;

		[Parameter]
		public IAttribute Attribute { get; set; }
		[Parameter]
		public EventCallback<string> RemoveAttribute { get; set; }

		private void AddBuffParam()
		{
			childComponent.AddBuffParam();
		}

		private async Task Remove(string attribute)
		{
			await RemoveAttribute.InvokeAsync(attribute);
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
