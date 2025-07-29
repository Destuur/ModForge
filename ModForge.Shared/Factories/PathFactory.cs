using ModForge.Shared.Models.Enums;
using ModForge.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModForge.Shared.Factories
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

		public static string CreateImportLocalizationPath(string prefix, Language language)
		{
			return Path.Combine(prefix, "Localization", language + "_xml.pak");
		}

		public static string CreateExportLocalizationPath(string prefix, string language, string modId)
		{
			return Path.Combine(prefix, "Mods", modId, "Localization", language + "_xml", "text__" + modId + ".xml");
		}

		public static IEnumerable<string> CreatePakPaths(string prefix)
		{
			var pakPaths = new List<string>();
			var allLanguages = Enum.GetValues(typeof(Language)).Cast<Language>();
			foreach (var language in allLanguages)
			{
				pakPaths.Add(CreateImportLocalizationPath(prefix, language));
			}
			return pakPaths;
		}

		public static string CreateModFolderPath(string prefix, string modId)
		{
			return Path.Combine(prefix, "Mods", modId);
		}

		public static string CreateModToPakPath(string prefix, string modId)
		{
			return Path.Combine(prefix, "Mods", modId, "Data");
		}

		public static string CreateStormFilePath(string prefix, string modId)
		{
			return Path.Combine(prefix, "Mods", modId, "Data", "Libs", "Storm");
		}
	}
}
