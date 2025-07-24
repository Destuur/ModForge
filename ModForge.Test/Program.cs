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
			var dudeString = "dude.xml";

			foreach (var task in storm.Tasks.TaskItems)
			{
				foreach (var source in task.Sources)
				{
					if (source.Path.Contains(dudeString) && source.Path.Contains("abilities"))
					{
						var dude = ReadStormFile(pakPath, source.Path);
					}
				}
			}
		}

		private static Storm ReadStormFile(string pakPath, string stormFile)
		{
			Storm storm = null;

			using (FileStream zipToOpen = new FileStream(pakPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{

				var normalizedStormFile = stormFile.Replace('\\', '/');

				foreach (var entry in archive.Entries)
				{
					if (entry.FullName.Contains(normalizedStormFile) == false)
					{
						continue;
					}

					string xml = entry.Open().ReadAllText();
					var serializer = new XmlSerializer(typeof(Storm));

					using var reader = new StringReader(xml);
					storm = (Storm)serializer.Deserialize(reader);
				}
			}
			return storm;
		}
	}
}