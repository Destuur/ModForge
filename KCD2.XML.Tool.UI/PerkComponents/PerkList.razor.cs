using KCD2.XML.Tool.Shared.Mods;
using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.UI.PerkComponents
{
	public partial class PerkList
	{
		[Inject]
		public ModService? Service { get; private set; }
		[Parameter]
		public IEnumerable<IModItem>? PerkItems { get; set; }

		public IEnumerable<IModItem> TakePerkItems(int count)
		{
			return PerkItems!.Take(count);
		}

		public void AddModItem(IModItem item)
		{
			Service!.AddItem(item);
		}
	}
}
