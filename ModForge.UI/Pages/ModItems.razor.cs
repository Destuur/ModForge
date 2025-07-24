using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModForge.Localizations;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using ModForge.UI.Components.ModItemComponents;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Pages
{
	public partial class ModItems
	{
		public RenderFragment? CustomRender { get; set; }
		private bool isOpen;

		[Inject]
		public ModService? ModService { get; private set; }
		[Parameter]
		public string? ModId { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		[Inject]
		public LocalizationService LocalizationService { get; set; }
		[Inject]
		public IconService IconService { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public ILogger<ModItems> Logger { get; set; }

		private RenderFragment CreateComponent(Type type, EventCallback<Type> changeChildContent, EventCallback toggledDrawer) => builder =>
		{
			builder.OpenComponent(0, type);
			builder.AddAttribute(1, "ChangeChildContent", changeChildContent);
			builder.AddAttribute(2, "ToggledDrawer", toggledDrawer);
			builder.CloseComponent();
		};

		private void OnChangeChildContent(Type type)
		{
			CustomRender = CreateComponent(type, EventCallback.Factory.Create<Type>(this, OnChangeChildContent), EventCallback.Factory.Create(this, ToggleDrawer));
		}

		private void ToggleDrawer()
		{
			isOpen = !isOpen;
		}

		private async Task ExitModding()
		{
			if (ModService.Mod.ModItems.Count == 0)
			{
				await ExecuteTwoButtonExitDialog();
			}
			else
			{
				await ExecuteThreeButtonExitDialog();
			}
		}

		private async Task ExecuteThreeButtonExitDialog()
		{
			var parameters = new DialogParameters<ExitDialog>
			{
				{ x => x.ContentText, "Do you really want to cancel your current modding process? Any unsaved changes will be lost." },
				{ x => x.CancelButton, "Continue Modding" },
				{ x => x.ExitButton, "Discard & Exit" },
				{ x => x.SaveAndExitButton, "Save Changes & Exit" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };

			var dialog = await DialogService.ShowAsync<ExitDialog>("Cancel Modding", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled)
			{
				return;
			}

			if ((bool)(result.Data ?? false))
			{
				SaveMod();
			}

			ModService.ClearCurrentMod();
			NavigationManager.NavigateTo($"/");
		}

		private async Task ExecuteTwoButtonExitDialog()
		{
			var parameters = new DialogParameters<TwoButtonExitDialog>()
			{
				{ x => x.ContentText, "Are you sure you want to cancel your current modding process?" },
				{ x => x.CancelButton, "Cancel" },
				{ x => x.ExitButton, "Discard & Exit" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };

			if (DialogService is null)
			{
				Logger?.LogError("DialogService is null. Cannot show discard changes dialog.");
				return;
			}

			var dialog = await DialogService.ShowAsync<TwoButtonExitDialog>("No Epic Mod Today?", parameters, options);
			var result = await dialog.Result;


			if (result.Canceled)
			{
				return;
			}

			NavigationManager.NavigateTo($"/");
		}

		private async Task GetHelp()
		{
			var parameters = new DialogParameters<HelpDialog>()
			{
				{ x => x.ContentText, "This section may be helpful in the future. Who knows..." },
				{ x => x.ButtonText, "Stop yanking my pizzle!" },
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
			await DialogService.ShowAsync<HelpDialog>("Error 1403: Help not found!", parameters, options);
		}

		private async Task Checkout()
		{
			var parameters = new DialogParameters<MoreModItemsDialog>
			{
				{ x => x.ContentText, "Hast thou pulled enough pizzles? Depart then, and shape thy mod!" },
				{ x => x.ButtonText, "Create Mod" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<MoreModItemsDialog>("Create Your Mod", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled == false)
			{
				NavigationManager.NavigateTo($"/modoverview/{ModService.Mod.Id}");
			}
		}

		public void SaveMod()
		{
			Snackbar.Add(
				"Mod successfully saved",
				Severity.Success,
				config =>
				{
					config.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
				});
			ModService.ExportMod(ModService.Mod);
			ModService.ClearCurrentMod();
		}

		private void EditModItem(string id)
		{
			if (NavigationManager is null || string.IsNullOrEmpty(id))
			{
				return;
			}

			NavigationManager.NavigateTo($"/editing/moditem/{id}");
		}

		private void DeleteModItem(string id)
		{
			if (ModService is null || string.IsNullOrEmpty(id))
			{
				return;
			}
			var foundModItem = ModService.Mod.ModItems.FirstOrDefault(x => x.Id == id);

			if (foundModItem is null)
			{
				return;
			}

			ModService.Mod.ModItems.Remove(foundModItem);
			StateHasChanged();
		}

		private void SetLanguage()
		{
			var language = UserConfigurationService.Current.Language;
			var culture = string.IsNullOrEmpty(language) ? CultureInfo.CurrentCulture : new CultureInfo(UserConfigurationService.Current.Language);

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (ModService is null ||
				string.IsNullOrEmpty(ModId))
			{
				OnChangeChildContent(typeof(Perks));
				return;
			}
			ModService.TryGetModFromCollection(ModId);
			SetLanguage();
			OnChangeChildContent(typeof(Perks));
			isOpen = false;
		}
	}
}
