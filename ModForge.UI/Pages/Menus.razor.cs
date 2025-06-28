using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using ModForge.UI.Components.MenuComponents;
using MudBlazor;
using System.Globalization;

namespace ModForge.UI.Pages
{
	public partial class Menus
	{
		private ModCollection createdMods { get; set; }

		public RenderFragment? CustomRender { get; set; }

		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }
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

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			if (ModService is null)
			{
				return;
			}
			OnChangeChildContent(typeof(Dashboard));
		}
	}
}