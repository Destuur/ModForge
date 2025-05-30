using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Pages
{
	public partial class PerkEditingPage
	{
		private Perk perkCopy;

		[Parameter]
		public string Id { get; set; }
		[Inject]
		public XmlToJsonService XmlToJsonService { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();
			if (XmlToJsonService is null)
			{
				return;
			}

			var tempPerk = XmlToJsonService.Perks!.FirstOrDefault(x => x.Id == Id)!;
			perkCopy = Perk.GetDeepCopy(tempPerk);
		}
	}
}
