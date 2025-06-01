using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KCD2.ModForge.UI.Components.BuffComponents
{
	public partial class BuffListItem
	{
		private ModDescription? mod;
		private string languageKey = "en";

		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public LocalizationService? LocalizationService { get; set; }
		[Inject]
		public NavigationService? NavigationService { get; set; }
		[Parameter]
		public Buff? Buff { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();
			mod = ModService!.GetMod();
		}

		private async Task EditBuff(MouseEventArgs args)
		{
			if (NavigationService is null)
			{
				return;
			}
			await NavigationService.NavigateToAsync($"editing/buff/{Buff.Id}");
		}
	}
}
