using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KCD2.ModForge.UI.Components.BuffComponents
{
	public partial class BuffEditingItem
	{
		private IEnumerable<IAttribute> sortedAttributes => Buff.Attributes.OrderBy(x => x.Value.GetType().Name).Reverse();

		[Inject]
		public ModService? ModService { get; set; }
		[Parameter]
		public Buff Buff { get; set; }

		private async Task SaveBuff(MouseEventArgs args)
		{
			if (Buff is null)
			{
				return;
			}

			if (ModService is null)
			{
				return;
			}

			ModService.AddModItem(Buff);
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
