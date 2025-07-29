using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared;
using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Rules;
using ModForge.Shared.Models.STORM.Selectors;
using ModForge.Shared.Services;
using ModForge.UI.Components.StormComponents;
using MudBlazor;

namespace ModForge.UI.Components.DialogComponents
{
	public partial class RuleDialog
	{
		private OperationCategory category = new();
		private string name;
		private string comment;

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
		public Rule Rule { get; set; } = new();

		private void ApplyName()
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			Rule.Name = name.ReplaceWhiteSpace();
		}

		private void OnAddSelector(GenericSelector selector)
		{
			if (selector is null)
			{
				return;
			}
			Rule.Selectors.Add(selector);
		}

		private void OnRemoveSelector(GenericSelector selector)
		{
			if (selector == null)
			{
				return;
			}

			Rule.Selectors.Remove(selector);
		}

		private void AddSelector(string selector)
		{
			switch (selector)
			{
				case "and":
					Rule.Selectors.Add(new GenericSelector() { Name = "and", Children = new() { new GenericSelector() } });
					break;
				case "or":
					Rule.Selectors.Add(new GenericSelector() { Name = "or", Children = new() { new GenericSelector() } });
					break;
				case "not":
					Rule.Selectors.Add(new GenericSelector() { Name = "not", Children = new() { new GenericSelector() } });
					break;
				case "selector":
					Rule.Selectors.Add(new GenericSelector() { Name = "" });
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

		private void OnAddedOperations(List<GenericOperation> operations)
		{
			if (operations is null)
			{
				return;
			}
			Rule.Operations = operations;
		}

		private void Ok() => MudDialog.Close(DialogResult.Ok(Rule));
		private void Cancel() => MudDialog.Cancel();

		protected override void OnInitialized()
		{
			SelectedCategory = Storm.RuleCategories.FirstOrDefault().Value;
		}
	}

}