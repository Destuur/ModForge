using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.Shared.Factories
{
	public static class PathFactory
	{
		public static string CreateTablesPath(string prefix)
		{
			return Path.Combine(prefix, "Data\\Tables.pak");
		}

		public static string CreateScriptsPath(string prefix)
		{
			return Path.Combine(prefix, "Data\\Scripts.pak");
		}

		public static string CreateGameDataPath(string prefix)
		{
			return Path.Combine(prefix, "Data\\IPL_GameData.pak");
		}

		public static string CreateLocalizationPath(string prefix, Language language)
		{
			return Path.Combine([prefix, $"Localization\\{language}_xml.pak"]);
		}
	}
}
