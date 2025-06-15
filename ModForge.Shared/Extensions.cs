using System.IO.Compression;

namespace ModForge.Shared
{
	public static class Extensions
	{
		public static string GetEntryPath(this ZipArchiveEntry zipArchiveEntry)
		{
			var path = string.Empty;

			var directories = zipArchiveEntry.FullName.Split('/');

			// Prüfen, ob das letzte Element "xml" enthält und ggf. entfernen
			if (directories.Length > 0 && directories[^1].Contains("xml", StringComparison.OrdinalIgnoreCase))
			{
				directories = directories.Take(directories.Length - 1).ToArray();
			}

			// Den Pfad wieder zusammensetzen
			var pathWithoutXmlFile = string.Join("/", directories);

			path = pathWithoutXmlFile;

			return path;
		}
	}
}
