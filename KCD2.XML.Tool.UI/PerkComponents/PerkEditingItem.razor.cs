using KCD2.XML.Tool.Shared.Models;
using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KCD2.XML.Tool.UI.PerkComponents
{
	public partial class PerkEditingItem
	{
		[Parameter]
		public Perk? Perk { get; set; }
		[Inject]
		public ModService? ModService { get; set; }

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
