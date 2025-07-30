using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModForge.Localizations;
using ModForge.Shared.Models.STORM.Rules;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using ModForge.UI.Components.MenuComponents;
using MudBlazor;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class NewRules
	{
		private bool isLoaded;

		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Parameter]
		public string ModId { get; set; }
		[Parameter]
		public EventCallback ToggledDrawer { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public ILogger<Loadouts> Logger { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
		public Rule SelectedRule { get; set; }

		private async Task CreateNewRule()
		{
			var options = new DialogOptions()
			{
				CloseButton = true,
				NoHeader = false,
				BackgroundClass = "dialog-background",
				FullScreen = true,
				BackdropClick = false,
				CloseOnEscapeKey = false,
				FullWidth = true,
				Position = DialogPosition.Center
			};

			if (DialogService is null)
			{
				Logger?.LogError($"DialogService is null. Cannot show '{typeof(RuleDialog)}'.");
				return;
			}

			var dialog = await DialogService.ShowAsync<RuleDialog>("Create new rule", options);
			var result = await dialog.Result;


			if (result is null || result.Canceled || result.Data is null)
			{
				return;
			}
			if (result.Data is Rule rule)
			{
				ModService.Mod.StormRules.Add(rule);
			}

		}

		private void SelectRule(Rule rule)
		{
			if (rule is null)
			{
				return;
			}
			if (SelectedRule == rule)
			{
				SelectedRule = null;
				StateHasChanged();
				return;
			}
			SelectedRule = rule;
			StateHasChanged();
		}

		protected override void OnInitialized()
		{
			ModService.TryGetModFromCollection(ModId);
			isLoaded = true;
		}
	}
}