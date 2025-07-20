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
		public List<Type> GetWeaponClasses()
		{
			return new List<Type>()
			{
				typeof(MeleeWeaponClass),
				typeof(MissileWeaponClass)
			};
		}

		public List<Type> GetWeaponTypes()
		{
			return new List<Type>()
			{
				typeof(MeleeWeapon),
				typeof(MissileWeapon),
				typeof(Ammo)
			};
		}

		public List<Type> GetArmorTypes()
		{
			return new List<Type>()
			{
				typeof(Hood),
				typeof(Armor),
				typeof(Helmet),
			};
		}

		public List<Type> GetConsumableTypes()
		{
			return new List<Type>()
			{
				typeof(Food),
				typeof(Poison)
			};
		}

		public List<Type> GetCraftingMaterialsTypes()
		{
			return new List<Type>()
			{
				typeof(Herb),
				typeof(CraftingMaterial)
			};
		}

		public List<Type> GetMiscTypes()
		{
			return new List<Type>()
			{
				typeof(NPCTool),
				typeof(MiscItem),
				typeof(Document),
				typeof(Die),
				typeof(ItemAlias),
				typeof(QuickSlotContainer),
				typeof(DiceBadge),
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

			foreach (var type in GetWeaponClasses())
			{
				dictionary.Add(type, new()
				{
					{ "weapon_class", Path.Combine("Data", "Tables.pak") }
				});
			}

			foreach (var type in GetWeaponTypes())
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

			foreach (var type in GetArmorTypes())
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

			foreach (var type in GetConsumableTypes())
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

			foreach (var type in GetCraftingMaterialsTypes())
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

			foreach (var type in GetMiscTypes())
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
		public string GameDataPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Data\\IPL_GameData.pak";
		public string GermanLocalizationPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Localization\\German_xml.pak";
		public string EnglishLocalizationPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Localization\\English_xml.pak";
		public string IconPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Data\\IPL_GameData.pak";
		public string ModPath() => "G:\\SteamLibrary\\steamapps\\common\\KCD2Mod\\Mods";
		public string KCD2ModsPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Mods";
		public string GameDirectory() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2";
		#endregion
	}
}
