using ModForge.Shared.Models.STORM;
using System.IO.Compression;

namespace ModForge.Shared.Converter
{
	public class PakReader : IDisposable
	{
		private readonly ZipArchive _archive;
		private readonly Dictionary<string, ZipArchiveEntry> _entries;

		public PakReader(string pakPath)
		{
			var stream = new FileStream(pakPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
			_archive = new ZipArchive(stream, ZipArchiveMode.Read);
			_entries = _archive.Entries.ToDictionary(e => e.FullName.Replace('\\', '/'), StringComparer.OrdinalIgnoreCase);
		}

		public string ReadFile(string nameOrPath)
		{
			var normalizedPath = nameOrPath.Replace('\\', '/');

			// Direkter Versuch
			if (_entries.TryGetValue(normalizedPath, out var entry))
			{
				using var reader = new StreamReader(entry.Open());
				return reader.ReadToEnd();
			}

			// Fallback: Endet auf Pfad (z. B. für relative Pfade)
			var fallback = _entries.Values
				.FirstOrDefault(e => e.FullName.EndsWith(normalizedPath, StringComparison.OrdinalIgnoreCase));

			if (fallback != null)
			{
				using var reader = new StreamReader(fallback.Open());
				return reader.ReadToEnd();
			}

			// Logge zur Fehlersuche
			Console.WriteLine($"[WARN] Could not find storm file for path: {normalizedPath}");
			return null;
		}


		public Storm ReadStorm(string virtualPath)
		{
			var xml = ReadFile(virtualPath);
			if (xml == null) return null;

			var category = virtualPath.Split('/')[0];

			var parser = new StormParser();
			return parser.Parse(xml, category);
		}

		public void Dispose()
		{
			_archive.Dispose();
		}
	}
}
