using ModForge.Shared.Models.ModItems;

namespace ModForge.Shared
{
	public class ToolResources
	{
		public ToolResources()
		{

		}

		public static ToolResources Keys { get; set; } = new ToolResources();

		#region Valid Methods
		public Dictionary<Type, Dictionary<string, string>> Endpoints() => new()
		{
			{ typeof(Perk), new()
				{
					{ "perk__combat", Path.Combine("Data", "Tables.pak")},
					{ "perk__hardcore", Path.Combine("Data", "Tables.pak")},
					{ "perk__kcd2", Path.Combine("Data", "Tables.pak")},
				}
			},
			{ typeof(Buff), new()
				{
					{  "buff", Path.Combine("Data", "Tables.pak")},
					{  "buff__alchemy", Path.Combine("Data", "Tables.pak")},
					{  "buff__perk", Path.Combine("Data", "Tables.pak")},
					{  "buff__perk_hardcore", Path.Combine("Data", "Tables.pak")},
					{  "buff__perk_kcd1", Path.Combine("Data", "Tables.pak")},
				}
			},
		};
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
