using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModDashboardButtonsComponent
	{
		[Inject]
		public NavigationManager Navigation { get; set; }

		private void GoToSettings()
		{
			Navigation.NavigateTo("/settings");
		}

		private void GoToNewMod()
		{
			Navigation.NavigateTo("/newmod");
		}

		private void GoToManageMods()
		{
			Navigation.NavigateTo("/settings");
		}
	}
}
