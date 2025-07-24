using System.IO.Compression;
using System.Text;

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

		public static string ReadAllText(this Stream stream)
		{
			using var reader = new StreamReader(stream, Encoding.UTF8, true);
			return reader.ReadToEnd();
		}

		public static async Task<string> ReadAllTextAsync(this Stream stream)
		{
			using var reader = new StreamReader(stream, Encoding.UTF8, true);
			return await reader.ReadToEndAsync();
		}
	}
}
