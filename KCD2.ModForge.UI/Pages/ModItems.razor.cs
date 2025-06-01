using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KCD2.ModForge.UI.Pages
{
	public partial class ModItems
	{
		private int selectedTab = 0;

		[Inject]
		public ModService? ModService { get; private set; }
		[Parameter]
		public string? ModId { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (ModService is null ||
				string.IsNullOrEmpty(ModId))
			{
				return;
			}
			ModService.TrySetMod(ModId);
			await base.OnInitializedAsync();
		}
	}
}
