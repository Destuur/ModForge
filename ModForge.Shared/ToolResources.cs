using ModForge.Shared.Models.ModItems;
using System.Collections.Generic;

namespace ModForge.Shared
{
	public class ToolResources
	{
		public ToolResources()
		{

		}

		public static ToolResources Keys { get; set; } = new ToolResources();

		#region Valid Methods
		public List<Type> GetItemTypes()
		{
			return new List<Type>()
			{
				typeof(MeleeWeapon),
				typeof(NPCTool),
				typeof(MiscItem),
				typeof(Hood),
				typeof(Armor),
				typeof(MissileWeapon),
				typeof(Document),
				typeof(Herb),
				typeof(Food),
				typeof(Helmet),
				typeof(Die),
				typeof(Ammo),
				typeof(ItemAlias),
				typeof(QuickSlotContainer),
				typeof(DiceBadge),
				typeof(CraftingMaterial),
				typeof(Poison),
				typeof(PickableItem),
				typeof(Key),
				typeof(Money),
				typeof(KeyRing)
			};
		}

		public Dictionary<Type, Dictionary<string, string>> Endpoints()
		{
			Dictionary<Type, Dictionary<string, string>> dictionary = new()
			{
				{
					typeof(Perk), new()
					{
						{ "perk__combat", Path.Combine("Data", "Tables.pak")},
						{ "perk__hardcore", Path.Combine("Data", "Tables.pak")},
						{ "perk__kcd2", Path.Combine("Data", "Tables.pak")},
					}
				},
				{
					typeof(Buff), new()
					{
						{  "buff", Path.Combine("Data", "Tables.pak")},
						{  "buff__alchemy", Path.Combine("Data", "Tables.pak")},
						{  "buff__perk", Path.Combine("Data", "Tables.pak")},
						{  "buff__perk_hardcore", Path.Combine("Data", "Tables.pak")},
						{  "buff__perk_kcd1", Path.Combine("Data", "Tables.pak")},
					}
				}
			};

			foreach (var type in GetItemTypes())
			{
				dictionary.Add(type, new()
				{
					{ "item", Path.Combine("Data", "Tables.pak") },
					{ "item__alchemy", Path.Combine("Data", "Tables.pak") },
					{ "item__aux", Path.Combine("Data", "Tables.pak") },
					{ "item__deprecated", Path.Combine("Data", "Tables.pak") },
					{ "item__dlc", Path.Combine("Data", "Tables.pak") },
					{ "item__horse", Path.Combine("Data", "Tables.pak") },
					{ "item__rewards", Path.Combine("Data", "Tables.pak") },
					{ "item__system", Path.Combine("Data", "Tables.pak") },
					{ "item__unique", Path.Combine("Data", "Tables.pak") },
				});
			}

			return dictionary;
		}
		#endregion

		#region Test Methods
		public string ModId() => "test_mod";
		public string TablesPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Data\\Tables.pak";
		public string GermanLocalizationPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Localization\\German_xml.pak";
		public string EnglishLocalizationPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Localization\\English_xml.pak";
		public string IconPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Data\\IPL_GameData.pak";
		public string ModPath() => "G:\\SteamLibrary\\steamapps\\common\\KCD2Mod\\Mods";
		public string KCD2ModsPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Mods";
		public string GameDirectory() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2";
		#endregion
	}
}
