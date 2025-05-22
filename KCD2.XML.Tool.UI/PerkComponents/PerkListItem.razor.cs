using KCD2.XML.Tool.Shared.Models;
using KCD2.XML.Tool.Shared.Mods;
using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.UI.PerkComponents
{
	public partial class PerkListItem
	{
		private ModDescription? mod;

		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public LocalizationService? LocalizationService { get; set; }
		[Parameter]
		public IModItem? Perk { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();
			mod = ModService!.GetMod();
			if (Perk is Perk perk)
			{
				if (perk.Attributes.TryGetValue("perk_ui_desc", out string uiDesc))
				{
					perk.Localizations.Add(LocalizationService!.GetLocalization(uiDesc));
				}

				if (perk.Attributes.TryGetValue("perk_ui_lore_desc", out string uiLoreDesc))
				{
					perk.Localizations.Add(LocalizationService!.GetLocalization(uiLoreDesc));
				}
			}
		}
	}
}
