using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KCD2.ModForge.UI.Components.PerkComponents
{
	public partial class PerkList
	{
		private bool isOpen;
		private string languageKey = "en";

		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public XmlToJsonService? XmlToJsonService { get; set; }
		public IList<IModItem> PerkItems { get; set; } = new List<IModItem>();
		public string? SearchPerk { get; set; }


		public void ToggleDrawer()
		{
			isOpen = !isOpen;
		}

		public IEnumerable<IModItem> TakePerkItems(int count)
		{
			if (XmlToJsonService is null)
			{
				return null!;
			}
			return XmlToJsonService.Perks!.ToList().Take(count);
		}

		public string GetForgeIcon()
		{
			if (ModService.GetCurrentMod().ModItems.Count == 0)
			{
				return "images\\Icons\\forgeicon.png";
			}

			return "images\\Icons\\forgeicon2.png";
		}

		public void AddModItem(IModItem item)
		{
			ModService!.AddModItem(item);
		}

		public void SearchPerks()
		{
			if (XmlToJsonService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(SearchPerk))
			{
				PerkItems = XmlToJsonService.Perks.ToList();
				return;
			}

			string filter = SearchPerk;

			var filtered = XmlToJsonService.Perks.Where(x =>
				(!string.IsNullOrEmpty(x.Id) && x.Id.Contains(filter, StringComparison.OrdinalIgnoreCase)) ||

				(x.Localization.Names != null &&
				 x.Localization.Names.TryGetValue(languageKey, out var names) &&
				 names.Values.Any(v => v.Contains(filter, StringComparison.OrdinalIgnoreCase))) ||

				(x.Localization.Descriptions != null &&
				 x.Localization.Descriptions.TryGetValue(languageKey, out var descriptions) &&
				 descriptions.Values.Any(v => v.Contains(filter, StringComparison.OrdinalIgnoreCase))) ||

				(x.Localization.LoreDescriptions != null &&
				 x.Localization.LoreDescriptions.TryGetValue(languageKey, out var lores) &&
				 lores.Values.Any(v => v.Contains(filter, StringComparison.OrdinalIgnoreCase)))
			);

			PerkItems = filtered.ToList();
		}

		protected override async Task OnInitializedAsync()
		{
			if (XmlToJsonService is null)
			{
				return;
			}

			await base.OnInitializedAsync();
			PerkItems = XmlToJsonService.Perks.ToList();
		}

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			if (PerkItems is null)
			{
				return;
			}
		}
	}
}
