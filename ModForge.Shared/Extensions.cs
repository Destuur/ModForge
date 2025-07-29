using Microsoft.Extensions.Logging;
using ModForge.Shared.Converter;
using ModForge.Shared.Factories;
using ModForge.Shared.Models.STORM;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using static ModForge.Shared.Models.STORM.Operations.OperationParser;

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

		public static StormDto GetInitialStormDto(this Source source, string rootPath, string category = null)
		{
			var combined = source.Path.Replace('\\', '/');
			var stormDto = new StormDto();
			stormDto.DataPoint = DataPointFactory.CreateDataPoint(rootPath, combined, typeof(Storm));
			stormDto.Category = category;
			stormDto.Id = Guid.NewGuid().ToString();
			return stormDto;
		}

		public static StormDto ReadStormFile(this StormDto storm)
		{
			var dataPoint = storm.DataPoint;

			if (dataPoint == null)
				return null;

			using var pakReader = new PakReader(dataPoint.Path);

			var childStorm = pakReader.ReadStorm(dataPoint.Endpoint);
			if (childStorm != null)
			{
				storm.Rules = childStorm.Rules;
				storm.Tasks = childStorm.Tasks;
				storm.Common = childStorm.Common;
				storm.CustomSelectors = childStorm.CustomSelectors;
				storm.CustomOperations = childStorm.CustomOperations;
			}
			return storm;
		}

		public static string CapitalizeFirstLetterOnly(this string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			return char.ToUpper(input[0]) + input.Substring(1).ToLower();
		}

		public static string ReplaceWhiteSpace(this string text)
		{
			var newString = text.Trim().ToLower().Split(' ');
			return string.Join('_', newString);
		}
	}
}
