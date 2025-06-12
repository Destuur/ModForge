using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModDashboardButtonsComponent
	{
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public XmlToJsonService XmlToJsonService { get; set; }

		public bool ValidateModItemList()
		{
			if (XmlToJsonService.Perks is null || XmlToJsonService.Buffs is null)
			{
				return true;
			}
			return false;
		}

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
			Navigation.NavigateTo("/manager");
		}
	}
}
