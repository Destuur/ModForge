namespace KCD2.XML.Tool.Shared
{
	public class ToolRessources
	{
		public ToolRessources()
		{

		}

		public static ToolRessources Keys { get; set; } = new ToolRessources();

		public string ModId() => "test_mod";
		public string TablesPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Data\\Tables.pak";
		public string LocalizationPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Localization\\";
		public string ModPath() => "G:\\SteamLibrary\\steamapps\\common\\KingdomComeDeliverance2\\Mods\\";
	}
}
