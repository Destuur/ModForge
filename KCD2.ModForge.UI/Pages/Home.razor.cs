using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Pages
{
	public partial class Home
	{
		[Inject]
		public OrchestrationService? OrchestrationService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			if (OrchestrationService is null)
			{
				return;
			}

			await OrchestrationService.Initialize();
		}
	}
}
