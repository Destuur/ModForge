using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Enums;
using System.Text.RegularExpressions;
using static MudBlazor.Colors;

namespace ModForge.UI.Components.AttributeComponents
{
	public partial class BuffParamRow
	{

		private IEnumerable<string> enumNames = Enumerable.Empty<string>();
		private Dictionary<string, string> enumDisplayNames = new();
		private string currentEnumString = string.Empty;

		[Parameter]
		public BuffParam BuffParam { get; set; }
		[Parameter]
		public EventCallback<string> RemoveBuffParam { get; set; }

		private async Task<IEnumerable<string>> Search(string value, CancellationToken token)
		{
			await Task.Delay(5, token);

			var keys = BuffParamSerializer.GetAllKeys().ToList();
			var names = BuffParamSerializer.GetAllNames().ToList();
			var descriptions = BuffParamSerializer.GetAllDescriptions().ToList();

			if (string.IsNullOrEmpty(value))
				return keys;

			return keys.Where((key, i) =>
				key.Contains(value, StringComparison.InvariantCultureIgnoreCase) ||
				(i < names.Count && names[i].Contains(value, StringComparison.InvariantCultureIgnoreCase)) ||
				(i < descriptions.Count && descriptions[i].Contains(value, StringComparison.InvariantCultureIgnoreCase))
			);
		}

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
		}

		private string SplitCamelCase(string input)
		{
			return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
		}
	}
}
