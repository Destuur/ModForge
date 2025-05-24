namespace KCD2.XML.Tool.Shared
{
	public class ToolResources
	{
		public ToolResources()
		{

		}

		public static ToolResources Keys { get; set; } = new ToolResources();

		public string ModId() => "test_mod";
		public string TablesPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Data\\Tables.pak";
		public string GermanLocalizationPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Localization\\German_xml.pak";
		public string EnglishLocalizationPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Localization\\English_xml.pak";
		public string IconPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Data\\IPL_GameData.pak";
		public string ModPath() => "G:\\SteamLibrary\\steamapps\\common\\KCD2Mod\\Mods";
	}
}
