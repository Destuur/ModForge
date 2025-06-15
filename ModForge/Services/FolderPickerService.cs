using ModForge.Shared.Services;
using Ookii.Dialogs.Wpf;

namespace ModForge.Services
{
	public class FolderPickerService : IFolderPickerService
	{
		public Task<string?> PickFolderAsync()
		{
			var dialog = new VistaFolderBrowserDialog
			{
				Description = "Select target folder",
				UseDescriptionForTitle = true,
				ShowNewFolderButton = true
			};

			bool? result = dialog.ShowDialog();
			return Task.FromResult(result == true ? dialog.SelectedPath : null);
		}
	}
}
