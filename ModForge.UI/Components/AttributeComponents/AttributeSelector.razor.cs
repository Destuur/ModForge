using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Models.Abstractions;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;
using System.Globalization;
using System.Text.RegularExpressions;
using static MudBlazor.CategoryTypes;

namespace ModForge.UI.Components.AttributeComponents
{
	public partial class AttributeSelector
	{
		private BuffParamsAttribute? childComponent;

		[Inject]
		public ILogger<AttributeSelector> Logger { get; set; }
		[Inject]
		public IDialogService? DialogService { get; set; }
		[Parameter]
		public IAttribute? Attribute { get; set; }
		[Parameter]
		public EventCallback<string> RemoveAttribute { get; set; }
		[Parameter]
		public EventCallback<string> ResetedValue { get; set; }

		private void ResetValue(string key)
		{
			ResetedValue.InvokeAsync(key);
		}

		private async Task AddBuffParam()
		{
			var parameters = new DialogParameters<BuffParamDialog>()
			{
				{ x => x.ButtonText, "Apply" },
				{ x => x.CancleText, "Cancle" },
				{ x => x.Title, "Select Buff Parameter" }
			};

			var options = new DialogOptions() 
			{ 
				CloseButton = true, 
				MaxWidth = MaxWidth.Medium, 
				NoHeader = true,
				BackgroundClass = "dialog-background"
			};

			if (DialogService is null)
			{
				Logger?.LogError($"DialogService is null. Cannot show '{typeof(BuffParamDialog)}'.");
				return;
			}

			var dialog = await DialogService.ShowAsync<BuffParamDialog>("No Epic Mod Today?", parameters, options);
			var result = await dialog.Result;


			if (result is null || result.Canceled || result.Data is null)
			{
				return;
			}

			childComponent!.AddBuffParam(result.Data.ToString()!);
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
