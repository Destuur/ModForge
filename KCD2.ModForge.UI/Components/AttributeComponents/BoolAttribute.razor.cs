using KCD2.ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KCD2.ModForge.UI.Components.AttributeComponents
{
	public partial class BoolAttribute
	{
		[CascadingParameter]
		public IAttribute Attribute { get; set; }

		// Bindet an MudSelect mit string-Werten ("True"/"False")
		string CurrentValueString
		{
			get => (Attribute.Value is bool b && b) ? "True" : "False";
			set => Attribute.Value = string.Equals(value, "True", StringComparison.OrdinalIgnoreCase);
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
