using KCD2.ModForge.Shared;
using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Factories;
using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.User;
using KCD2.ModForge.Shared.Services;
using System;
using System.Diagnostics;

namespace KCD2.ModForge.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var buffexclusive = new BuffExclusivityId("test", "1");
			var name = buffexclusive.Name;
			var value = buffexclusive.Value;

			// Wubba lubba dub dub
			var attribute1 = AttributeFactory.CreateAttribute("buff_params", "ewd*0.85,dew*2");
			var attribute2 = AttributeFactory.CreateAttribute("buff_ui_visibility_id", "1");
			var attribute3 = AttributeFactory.CreateAttribute("level", "12");
			var attribute4 = AttributeFactory.CreateAttribute("autolearnable", "true");

			var buffParams = new BuffParams("test", "ewd*0.85,dew*2");
			var buffParamsName = buffParams.Name;
			var buffParamsValue = buffParams.Value;

			var watch = Stopwatch.StartNew();
			var userService = new UserConfigurationService();
			var gameDirectory = userService.Current.GameDirectory;
			var path = PathFactory.CreateLocalizationPath(gameDirectory, Language.German);

			var xmlPerkAdapter = new XmlAdapter<Perk>(userService);
			var perks = xmlPerkAdapter.ReadAsync().Result;

			var xmlBuffAdapter = new XmlAdapter<Buff>(userService);
			var buffs = xmlBuffAdapter.ReadAsync().Result;

			var localizationAdapter = new LocalizationAdapter();
			var pakPaths = new List<string>();
			var allLanguages = Enum.GetValues(typeof(Language)).Cast<Language>();
			foreach (var language in allLanguages)
			{
				pakPaths.Add(PathFactory.CreateLocalizationPath(gameDirectory, language));
			}
			var localizations = localizationAdapter.LoadAllLocalizationsFromPaks(pakPaths);



			string? GetAttributeValue(IEnumerable<IAttribute> attributes, params string[] names)
			{
				foreach (var name in names)
				{
					var attr = attributes.FirstOrDefault(a => a.Name == name);
					if (attr != null)
						return attr.Value?.ToString();
				}
				return null;
			}

			var languageKey = "de"; // oder eine andere Sprache

			foreach (var perk in perks)
			{
				foreach (var language in LocalizationAdapter.LanguageMap.Values)
				{
					if (localizations.TryGetValue(language, out var langDict))
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


			try
			{
				foreach (var buff in buffs)
				{
					foreach (var language in LocalizationAdapter.LanguageMap.Values)
					{
						if (localizations.TryGetValue(language, out var langDict))
						{
							var descKey = GetAttributeValue(buff.Attributes, "buff_desc");
							var loreDescKey = GetAttributeValue(buff.Attributes, "slot_buff_ui_name");
							var nameKey = GetAttributeValue(buff.Attributes, "buff_ui_name");

							if (descKey != null && langDict.TryGetValue(descKey, out var desc))
								buff.Localization.Descriptions[language] = desc;
							if (loreDescKey != null && langDict.TryGetValue(loreDescKey, out var loreDesc1))
								buff.Localization.LoreDescriptions[language] = loreDesc1;
							if (nameKey != null && langDict.TryGetValue(nameKey, out var name2))
								buff.Localization.Names[language] = name2;
						}
					}
				}
			}
			catch (Exception)
			{

				throw;
			}

			watch.Stop();
			Console.WriteLine(path);


			Console.ReadLine();
		}

	}
}
