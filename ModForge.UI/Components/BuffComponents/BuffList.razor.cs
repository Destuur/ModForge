using ModForge.Shared.Factories;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ModForge.UI.Components.BuffComponents
{
	public partial class BuffList
	{
		private ModDescription? mod;
		private string languageKey = "en";

		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public XmlToJsonService? XmlToJsonService { get; set; }
		public IList<IModItem> BuffItems { get; set; } = new List<IModItem>();
		public string? SearchBuff { get; set; }

		public string GetForgeIcon()
		{
			if (ModService.GetCurrentMod().ModItems.Count == 0)
			{
				return "images\\Icons\\forgeicon.png";
			}

			return "images\\Icons\\forgeicon2.png";
		}

		public IEnumerable<IModItem> TakeBuffItems(int count)
		{
			if (XmlToJsonService is null)
			{
				return null!;
			}
			return XmlToJsonService.Buffs!.ToList().Take(count);
		}

		public void AddModItem(IModItem item)
		{
			ModService!.AddModItem(item);
		}

		public void SearchBuffs()
		{
			if (XmlToJsonService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(SearchBuff))
			{
				BuffItems = XmlToJsonService.Buffs.ToList();
				return;
			}

			string filter = SearchBuff;

			var filtered = XmlToJsonService.Buffs.Where(x =>
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

			BuffItems = filtered.ToList();
		}

		protected override async Task OnInitializedAsync()
		{
			if (XmlToJsonService is null)
			{
				return;
			}

			await base.OnInitializedAsync();
			BuffItems = XmlToJsonService.Buffs.Where(x => x.Localization == null || (x.Localization.Names == null || x.Localization.Names.Count == 0) == false).ToList();
		}

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			if (BuffItems is null)
			{
				return;
			}
		}
	}
}
