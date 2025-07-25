using ModForge.Shared;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.Data;
using ModForge.Shared.Models.STORM;
using System.IO.Compression;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ModForge.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			var pakPath = @"G:\SteamLibrary\steamapps\common\KingdomComeDeliverance2\Data\IPL_GameData.pak";
			var storm = ReadStormFile(pakPath, "storm.xml");
			List<Storm> stormFiles = new();

			foreach (var task in storm.Tasks)
			{
				// Ich gehe davon aus, dass du in Task eine Sources-Liste hast (sonst anpassen)
				foreach (var source in task.Sources)
				{
					stormFiles.Add(ReadStormFile(pakPath, source.Path));
				}
			}

			var selectors = SelectorParser.SelectorAttributes;
			var operations = OperationParser.OperationAttributes;
		}

		private static Storm ReadStormFile(string pakPath, string stormFile)
		{
			using FileStream zipToOpen = new(pakPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
			using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Read);

			var normalizedStormFile = stormFile.Replace('\\', '/');

			foreach (var entry in archive.Entries)
			{
				if (!entry.FullName.Contains(normalizedStormFile))
					continue;

				using var stream = entry.Open();

				string xml = new StreamReader(entry.Open()).ReadToEnd();
				var parser = new StormParser();
				var storm = parser.Parse(xml);
				return storm;
			}

			return null; // Datei nicht gefunden
		}
	}
}