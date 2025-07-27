using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Models.STORM;
using ModForge.Shared.Models.STORM.Selectors;
using ModForge.Shared.Services;
using MudBlazor;

namespace ModForge.UI.Components.DialogComponents
{
	public partial class RuleDialog
	{
		private OperationCategory category = new();

		[CascadingParameter]
		private IMudDialogInstance MudDialog { get; set; }
		[Parameter]
		public string? ContentText { get; set; }
		[Parameter]
		public string? CancelButton { get; set; }
		[Parameter]
		public string? SaveAndExitButton { get; set; }
		[Parameter]
		public string? ExitButton { get; set; }
		[Inject]
		public StormService? Storm { get; set; }
		public OperationCategory? SelectedCategory { get; set; }
		public List<GenericSelector>? Selectors { get; set; } = new();

		private void AddSelector(string selector)
		{
			switch (selector)
			{
				case "and":
					Selectors.Add(new GenericSelector() { Name = "and" });
					break;
				case "or":
					Selectors.Add(new GenericSelector() { Name = "or" });
					break;
				case "not":
					Selectors.Add(new GenericSelector() { Name = "not" });
					break;
				case "selector":
					Selectors.Add(new GenericSelector() { Name = "" });
					break;
				default:
					break;
			}

			StateHasChanged();
		}

		private string GetSelectorTitle(ISelector selector)
		{
			if (selector is null)
			{
				return "No selector found";
			}

			if (selector is IConditionalSelector conditional)
			{
				return conditional.GetType().Name;
			}

			return "what?";
		}

		private void SaveAndExit() => MudDialog.Close(DialogResult.Ok(true));

		private void Exit() => MudDialog.Close(DialogResult.Ok(false));

		private void Cancel() => MudDialog.Cancel();
	}
}