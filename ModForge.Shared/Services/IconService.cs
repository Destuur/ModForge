using Microsoft.Extensions.Logging;
using ModForge.Shared.Converter;
using ModForge.Shared.Models.Abstractions;
using Pfim;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;

namespace ModForge.Shared.Services
{

	public class IconService
	{
		private readonly ILogger<IconService> logger;
		private readonly UserConfigurationService configService;

		public IconService(UserConfigurationService configService, ILogger<IconService> logger)
		{
			this.configService = configService;
			this.logger = logger;
		}

		public string? GetIcon(IModItem modItem)
		{
			if (modItem is null || modItem.Attributes is null)
			{
				return null;
			}

			var iconId = modItem.Attributes.FirstOrDefault(x => x.Name == "icon_id")!.Value.ToString();

			if (string.IsNullOrEmpty(iconId))
			{
				iconId = modItem.Attributes.FirstOrDefault(x => x.Name == "IconId")!.Value.ToString();
			}
			if (string.IsNullOrEmpty(iconId))
			{
				return null;
			}

			return GetBase64Icon(iconId);
		}

		public string? GetBase64Icon(string iconId)
		{
			string pakPath = Path.Combine(configService.Current.GameDirectory, "Data", "IPL_GameData.pak");
			string targetFilename = $"{iconId}";

			using FileStream zipStream = new(pakPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
			using ZipArchive archive = new(zipStream, ZipArchiveMode.Read);

			var entry = archive.Entries
				.FirstOrDefault(e =>
					e.FullName.Contains(targetFilename, StringComparison.OrdinalIgnoreCase) &&
					e.FullName.Contains("Libs/UI/Textures", StringComparison.OrdinalIgnoreCase));

			if (entry == null)
			{
				logger.LogWarning("Icon not found: {IconId}", iconId);
				return null;
			}

			try
			{
				using Stream ddsStream = entry.Open();
				using var pngStream = DdsConverter.ConvertToPngStream(ddsStream);
				var base64 = Convert.ToBase64String(pngStream.ToArray());
				return $"data:image/png;base64,{base64}";
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error converting DDS to PNG (iconId={IconId})", iconId);
				return null;
			}
		}
	}
}
