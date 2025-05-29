using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KCD2.ModForge.UI.Components.PerkComponents
{
	public partial class PerkEditingItem
	{
		[Inject]
		public ModService? ModService { get; set; }
		[Parameter]
		public Perk Perk { get; set; }

		private async Task SavePerk(MouseEventArgs args)
		{
			if (Perk is null)
			{
				return;
			}

			if (ModService is null)
			{
				return;
			}

			ModService.AddItem(Perk);
			await ModService.ExportMod();
		}

		private void ChangeAttribute(KeyValuePair<string, string> keyValuePair)
		{

		}

		private void ChangeLocalization(string value)
		{

		}
	}
}
