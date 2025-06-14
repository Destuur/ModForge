using KCD2.ModForge.Shared.Services;
using Ookii.Dialogs.Wpf;

namespace KCD2.ModForge.Wpf.Services
{
	public class FolderPickerService : IFolderPickerService
	{
		public Task<string?> PickFolderAsync()
		{
			var dialog = new VistaFolderBrowserDialog
			{
				Description = "Zielordner auswählen",
				UseDescriptionForTitle = true,
				ShowNewFolderButton = true
			};

			bool? result = dialog.ShowDialog();
			return Task.FromResult(result == true ? dialog.SelectedPath : null);
		}
	}
}
