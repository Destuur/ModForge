using KCD2.XML.Tool.Shared.Models;
using KCD2.XML.Tool.Shared.Mods;
using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace KCD2.XML.Tool.UI.PerkComponents
{
	public partial class PerkList
	{
		private ModDescription? mod;
		private string descAttribute = "perk_ui_desc";
		private string loreDescAttribute = "perk_ui_lore_desc";
		private string nameAttribute = "perk_ui_name";

		[Inject]
		public PerkService? PerkService { get; set; }
		[Inject]
		public LocalizationService? LocalizationService { get; set; }
		[Inject]
		public IconService? IconService { get; set; }
		[Inject]
		public ModService? ModService { get; private set; }
		public IEnumerable<IModItem>? PerkItems { get; set; }
		public string SearchPerk { get; set; }

		public IEnumerable<IModItem> TakePerkItems(int count)
		{
			return PerkItems!.Take(count);
		}

		public void AddModItem(IModItem item)
		{
			ModService!.AddItem(item);
		}

		public void SearchPerks()
		{
			PerkItems = PerkService.GetAllPerks();
			if (string.IsNullOrEmpty(SearchPerk))
			{
				PerkItems = PerkService.GetAllPerks();
				return;
			}

			if (PerkItems is IEnumerable<Perk> perks)
			{
				var filtered = perks.Where(x =>
				(!string.IsNullOrEmpty(x.Id) && x.Id.Contains(SearchPerk, StringComparison.OrdinalIgnoreCase)) ||
				(x.Localizations.Any(x => x.Value.Contains(SearchPerk, StringComparison.OrdinalIgnoreCase))) ||
				(x.Buffs.Any(x => x.Id.Contains(SearchPerk, StringComparison.OrdinalIgnoreCase)))
				).ToList();

				PerkItems = filtered;
			}
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			PerkItems = PerkService!.GetAllPerks();
		}

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			if (PerkItems is null)
			{
				return;
			}

			foreach (var perkItem in PerkItems)
			{
				if (perkItem is Perk perk)
				{
					if (perk.Attributes.TryGetValue(descAttribute, out string perkDesc))
					{
						perk.Localizations.Add(LocalizationService!.GetLocalization(descAttribute, perkDesc));
					}

					if (perk.Attributes.TryGetValue(loreDescAttribute, out string perkLoreDesc))
					{
						perk.Localizations.Add(LocalizationService!.GetLocalization(loreDescAttribute, perkLoreDesc));
					}

					if (perk.Attributes.TryGetValue(nameAttribute, out string perkName))
					{
						perk.Localizations.Add(LocalizationService!.GetLocalization(nameAttribute, perkName));
					}
				}
			}
		}
	}
}
