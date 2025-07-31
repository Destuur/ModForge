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
using System.Text.RegularExpressions;

namespace ModForge.UI.Components.DialogComponents
{
	public partial class RuleDialog
	{
		private string name;
		private string comment;
		private OperationCategory? selectedCategory;
		private bool firstStepComplete;
		private bool editMode;
		private string[] ruleAddedMessages = new[]
		{
			"Huzzah! Thy rule hath been added to the codex.",
			"By royal decree, thy rule now standeth firm!",
			"The scribes have etched thy rule into the great tome!",
			"’Tis done! Thy rule is now part of the lore.",
			"Lo and behold! A new rule is born."
		};
		private DialogOptions options = new DialogOptions()
		{
			CloseButton = false,
			NoHeader = false,
			BackgroundClass = "dialog-background",
			FullScreen = false,
			BackdropClick = true,
			CloseOnEscapeKey = true,
			FullWidth = false,
			Position = DialogPosition.Center
		};

		[CascadingParameter]
		private IMudDialogInstance MudDialog { get; set; }
		[Parameter]
		public string RuleId { get; set; }
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
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		public Rule Rule { get; set; } = new();
		public OperationCategory? SelectedCategory
		{
			get => selectedCategory;
			set
			{
				if (value is null)
				{
					return;
				}
				selectedCategory = value;
				Rule.Category = selectedCategory.Name;
			}
		}

		private void ApplyName()
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			Rule.Name = name.ReplaceWhiteSpace();
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

		private string ValidateRuleName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				firstStepComplete = false;
				return "The rule name can't be empty.";
			}

			var regex = new Regex(@"^[a-zA-Z\s]+$");

			if (!regex.IsMatch(value))
			{
				firstStepComplete = false;
				return "The rule name may only contain letters and spaces. No numbers or special characters allowed.";
			}

			var ruleString = name.Trim().ToLower().Split(' ');
			var tempRuleString = string.Join('_', ruleString);

			if (ModService.Mod.StormRules.FirstOrDefault(x => x.Name == tempRuleString) is not null && editMode == false)
			{
				firstStepComplete = false;
				return "A rule with this name is already in your collection.";
			}

			firstStepComplete = true;
			return string.Empty;
		}

		private async Task OnPreviewInteraction(StepperInteractionEventArgs arg)
		{
			switch (arg.Action)
			{
				case StepAction.Activate:
					await ControlStepNavigation(arg);
					break;
				case StepAction.Complete:
					await ControlStepCompletion(arg);
					break;
				case StepAction.Skip:
					break;
				case StepAction.Reset:
					break;
			}
		}

		private async Task ControlStepCompletion(StepperInteractionEventArgs arg)
		{

			switch (arg.StepIndex)
			{
				case 0:
					if (firstStepComplete != true)
					{
						await DialogService.ShowMessageBox("Error", "Thy rule hath no name, good sir!", "OK", null, null, options);
						arg.Cancel = true;
					}
					break;
				case 1:
					if (Rule.Selectors.Count == 0)
					{
						await DialogService.ShowMessageBox("Error", "Thou art missing a selector, milord.", "OK", null, null, options);
						arg.Cancel = true;
					}
					if (Rule.Selectors.Any(x => string.IsNullOrEmpty(x.Name)) ||
						Rule.Selectors.Where(x => x.Children is not null && x.Children.Count > 0).SelectMany(x => x.Children).Any(child => string.IsNullOrWhiteSpace(child.Name)))
					{
						await DialogService.ShowMessageBox("Error", "A nameless selector lurketh among thy ranks!", "OK", null, null, options);
						arg.Cancel = true;
					}
					break;
				case 2:
					if (Rule.Operations.Count == 0 || Rule.Operations.Any(x => string.IsNullOrEmpty(x.Name)))
					{
						await DialogService.ShowMessageBox("Error", "Without an operation, this rule is but an empty scroll!", "OK", null, null, options);
						arg.Cancel = true;
					}
					else
					{
						Snackbar.Add(ruleAddedMessages[Random.Shared.Next(0, 5)], Severity.Success, config =>
						{
							config.VisibleStateDuration = 2000;
							config.Icon = Icons.Material.Filled.ThumbUp;
						});
						Ok();
					}
					break;
			}
		}

		private async Task ControlStepNavigation(StepperInteractionEventArgs arg)
		{
			switch (arg.StepIndex)
			{
				case 1:
					if (firstStepComplete != true)
					{
						await DialogService.ShowMessageBox("Error", "Finish step 1 first");
						arg.Cancel = true;
					}
					break;
				default:
					break;
			}
		}

		private void Ok() => MudDialog.Close(DialogResult.Ok(Rule));

		protected override void OnInitialized()
		{
			if (string.IsNullOrEmpty(RuleId) == false)
			{
				var foundRule = ModService.Mod.StormRules.FirstOrDefault(x => x.Id == RuleId);
				Rule = new Rule();
				Rule.Name = foundRule.Name;
				Rule.Id = foundRule.Id;
				Rule.Selectors = foundRule.Selectors;
				Rule.Operations = foundRule.Operations;
				Rule.Comment = foundRule.Comment;
				Rule.Category = foundRule.Category;
				Rule.Mode = foundRule.Mode;
				name = Rule.Name;
				comment = Rule.Comment;
				selectedCategory = Storm.RuleCategories.Values.FirstOrDefault(x => x.Name == Rule.Category);
				editMode = true;
				firstStepComplete = true;
				return;
			}

			SelectedCategory = Storm.RuleCategories.FirstOrDefault().Value;
		}
	}

}