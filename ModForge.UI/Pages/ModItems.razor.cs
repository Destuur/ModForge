using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using ModForge.UI.Components.ModItemComponents;
using MudBlazor;

namespace ModForge.UI.Pages
{
	public partial class ModItems
	{
		public RenderFragment? CustomRender { get; set; }
		private int selectedTab = 0;

		[Inject]
		public ModService? ModService { get; private set; }
		[Parameter]
		public string? ModId { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }

		private RenderFragment CreateComponent(Type type, EventCallback<Type> changeChildContent) => builder =>
		{
			builder.OpenComponent(0, type);
			builder.AddAttribute(1, "ChangeChildContent", changeChildContent);
			builder.CloseComponent();
		};

		private void OnChangeChildContent(Type type)
		{
			CustomRender = CreateComponent(type, EventCallback.Factory.Create<Type>(this, OnChangeChildContent));
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
				NavigationManager.NavigateTo("/modoverview");
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
			ModService.ClearCurrentMod();
			NavigationManager.NavigateTo("/");
		}

		private async Task Cancel()
		{
			var parameters = new DialogParameters<ChangesDetectedDialog>
			{
				{ x => x.ContentText, "Are you sure you want to cancel the modding?" },
				{ x => x.ButtonText, "I am sure" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<ChangesDetectedDialog>("Cancel Modding", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled == false)
			{
				ModService.ClearCurrentMod();
				NavigationManager.NavigateTo($"/");
			}
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (ModService is null ||
				string.IsNullOrEmpty(ModId))
			{
				return;
			}
			ModService.TryGetModFromCollection(ModId);

			OnChangeChildContent(typeof(PerkItems));
		}
	}
}
