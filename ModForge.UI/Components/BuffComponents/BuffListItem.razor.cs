using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;

namespace ModForge.UI.Components.BuffComponents
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
		public NavigationManager Navigation { get; set; }
		[Inject]
		public ILogger<BuffListItem> Logger { get; set; }
		[Parameter]
		public Buff? Buff { get; set; }
		[Parameter]
		public string Placeholder { get; set; } = "No localization data available";

		protected override void OnInitialized()
		{
			base.OnInitialized();
			mod = ModService!.GetCurrentMod();
		}

		private void EditBuff(MouseEventArgs args)
		{
			if (Navigation is null)
			{
				Logger?.LogWarning("Navigation service is null, cannot navigate to buff editor.");
				return;
			}

			if (Buff is null || Buff.Id == null)
			{
				Logger?.LogWarning("Buff or Buff.Id is null, cannot navigate.");
				return;
			}

			Navigation.NavigateTo($"editing/buff/{Buff.Id}");
		}
	}
}
