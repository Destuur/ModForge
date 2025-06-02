using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Pages
{
	public partial class Settings
	{
		[Inject]
		public IFolderPickerService? FolderPickerService { get; set; }
		[Inject]
		public UserConfigurationService? UserConfigurationService { get; set; }
		[Inject]
		public XmlToJsonService? XmlToJsonService { get; set; }

		private async Task SelectGameDirectory()
		{
			if (FolderPickerService is null)
			{
				return;
			}
			if (UserConfigurationService is null)
			{
				return;
			}

			var selected = await FolderPickerService.PickFolderAsync();
			if (!string.IsNullOrWhiteSpace(selected))
			{
				UserConfigurationService.Current!.GameDirectory = selected;
			}
		}

		private async Task SelectNexusModsDirectory()
		{
			if (FolderPickerService is null)
			{
				return;
			}
			if (UserConfigurationService is null)
			{
				return;
			}

			var selected = await FolderPickerService.PickFolderAsync();
			if (!string.IsNullOrWhiteSpace(selected))
			{
				UserConfigurationService.Current!.NexusModsDirectory = selected;
			}
		}

		public void ParseXmlFiles()
		{
			if (XmlToJsonService is null)
			{
				return;
			}
			XmlToJsonService.ConvertXmlToJsonAsync();
		}
	}
}
