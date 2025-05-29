using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models;
using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.User;
using KCD2.ModForge.Shared.Mods;
using System;
using System.Text.Json;

namespace KCD2.ModForge.Shared.Services
{
	public class XmlToJsonService
	{
		private readonly XmlAdapter<Perk> perkAdapter;
		private readonly XmlAdapter<Buff> buffAdapter;
		private readonly LocalizationAdapter localizationAdapter;
		private Dictionary<string, Dictionary<string, string>> localizationCache;
		private readonly JsonAdapterOfT<Perk> jsonPerkAdapter;
		private readonly JsonAdapterOfT<Buff> jsonBuffAdapter;
		private readonly UserConfigurationService userConfigurationService;

		public XmlToJsonService(XmlAdapter<Perk> perkAdapter, XmlAdapter<Buff> buffAdapter, LocalizationAdapter localizationAdapter, JsonAdapterOfT<Perk> jsonPerkAdapter, JsonAdapterOfT<Buff> jsonBuffAdapter, UserConfigurationService userConfigurationService)
		{
			this.perkAdapter = perkAdapter;
			this.buffAdapter = buffAdapter;
			this.localizationAdapter = localizationAdapter;
			this.jsonPerkAdapter = jsonPerkAdapter;
			this.jsonBuffAdapter = jsonBuffAdapter;
			this.userConfigurationService = userConfigurationService;
		}

		public IList<Perk> Perks { get; private set; }
		public IList<Buff> Buffs { get; private set; }

		private async Task InitializeExport()
		{
			Perks = await perkAdapter.ReadAsync("");
			Buffs = await buffAdapter.ReadAsync("");
			localizationCache = localizationAdapter.LoadAllLocalizationsFromPaks();
		}

		private async Task AssignLocalizations()
		{
			await AssignPerkLocalizations();
			await AssignBuffLocalizations();
			return;
		}

		public async Task ExportXmlToJsonAsync()
		{
			await InitializeExport();
			await AssignLocalizations();
			await jsonPerkAdapter.WriteElements(Perks);
			await jsonBuffAdapter.WriteElements(Buffs);
		}

		public async Task<IEnumerable<Perk>> ReadPerkJsonFile(string filePath)
		{
			return await jsonPerkAdapter.ReadAsync(filePath);
		}

		public async Task<IEnumerable<Buff>> ReadBuffJsonFile(string filePath)
		{
			return await jsonBuffAdapter.ReadAsync(filePath);
		}

		private Task AssignPerkLocalizations()
		{

			foreach (var perk in Perks)
			{
				foreach (var language in LocalizationAdapter.LanguageMap.Values)
				{
					if (localizationCache.TryGetValue(language, out var langDict))
					{
						var descKey = GetAttributeValue(perk.Attributes, "perk_ui_desc");
						var loreDescKey = GetAttributeValue(perk.Attributes, "perk_ui_lore_desc");
						var nameKey = GetAttributeValue(perk.Attributes, "perk_ui_name");

						if (descKey != null && langDict.TryGetValue(descKey, out var desc))
							perk.Localization.Descriptions[language] = desc;
						if (loreDescKey != null && langDict.TryGetValue(loreDescKey, out var loreDesc))
							perk.Localization.LoreDescriptions[language] = loreDesc;
						if (nameKey != null && langDict.TryGetValue(nameKey, out var name1))
							perk.Localization.Names[language] = name1;
					}
				}
			}
			return Task.CompletedTask;
		}

		private Task AssignBuffLocalizations()
		{
			foreach (var buff in Buffs)
			{
				foreach (var language in LocalizationAdapter.LanguageMap.Values)
				{
					if (localizationCache.TryGetValue(language, out var langDict))
					{
						var descKey = GetAttributeValue(buff.Attributes, "buff_desc");
						var loreDescKey = GetAttributeValue(buff.Attributes, "slot_buff_ui_name");
						var uiNameKey = GetAttributeValue(buff.Attributes, "buff_ui_name");

						if (descKey != null && langDict.TryGetValue(descKey, out var buffDesc))
							buff.Localization.Descriptions[language] = buffDesc;
						if (loreDescKey != null && langDict.TryGetValue(loreDescKey, out var buffLoreDesc))
							buff.Localization.LoreDescriptions[language] = buffLoreDesc;
						if (uiNameKey != null && langDict.TryGetValue(uiNameKey, out var buffUiName))
							buff.Localization.Names[language] = buffUiName;
					}
				}
			}
			return Task.CompletedTask;
		}

		private string? GetAttributeValue(IEnumerable<IAttribute> attributes, params string[] names)
		{
			foreach (var name in names)
			{
				var attr = attributes.FirstOrDefault(a => a.Name == name);
				if (attr != null)
					return attr.Value?.ToString();
			}
			return null;
		}
	}
}
