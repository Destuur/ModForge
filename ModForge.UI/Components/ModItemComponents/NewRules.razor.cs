using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModForge.Localizations;
using ModForge.Shared.Models.STORM.Rules;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using ModForge.UI.Components.MenuComponents;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class NewRules
	{
		private bool isLoaded;
		private bool isOpen;

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
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }

		private void SetLanguage()
		{
			var language = UserConfigurationService.Current.Language;
			var culture = string.IsNullOrEmpty(language) ? CultureInfo.CurrentCulture : new CultureInfo(UserConfigurationService.Current.Language);

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		public async Task ToggleDrawer()
		{
			isOpen = !isOpen;
		}

		private async Task CreateNewRule(string id = null)
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

			var parameters = new DialogParameters<RuleDialog>()
			{
				{ x => x.RuleId, id },
			};

			if (DialogService is null)
			{
				Logger?.LogError($"DialogService is null. Cannot show '{typeof(RuleDialog)}'.");
				return;
			}

			var dialog = await DialogService.ShowAsync<RuleDialog>("Create new rule", parameters, options);
			var result = await dialog.Result;


			if (result is null || result.Canceled || result.Data is null)
			{
				return;
			}
			if (result.Data is Rule rule)
			{
				var foundRule = ModService.Mod.StormRules.FirstOrDefault(x => x.Name == rule.Name);
				if (foundRule is null)
				{
					ModService.Mod.StormRules.Add(rule);
					return;
				}
				foundRule = rule;
			}
		}

		private async Task EditRule(Rule rule)
		{
			if (rule is null)
			{
				return;
			}
			await CreateNewRule(rule.Id);
			StateHasChanged();
		}


		private void DeleteRule(Rule rule)
		{
			if (rule is null)
			{
				return;
			}
			ModService.Mod.StormRules.Remove(rule);
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
			SetLanguage();
			ModService.TryGetModFromCollection(ModId);
			isLoaded = true;
		}
	}
}