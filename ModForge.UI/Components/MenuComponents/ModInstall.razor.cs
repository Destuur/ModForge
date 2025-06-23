using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ModForge.Shared.Services;
using MudBlazor;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.IO.Compression;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModForge.UI.Components.MenuComponents
{
	public partial class ModInstall
	{
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }
		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }

		//TODO: Update mit Möglichkeit Thumbnails in der Mod zu hinterlegen!
		private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
		private string dragClass = DefaultDragClass;
		private readonly List<string> fileNames = new();
		private readonly List<IBrowserFile> files = new();
		private MudFileUpload<IReadOnlyList<IBrowserFile>>? fileUpload;
		private List<string> zipFormats = new List<string>()
		{
			".zip",
			".gzip",
			".rar",
			".7z",
			".tar"
		};

		private async Task ClearAsync()
		{
			await (fileUpload?.ClearAsync() ?? Task.CompletedTask);
			fileNames.Clear();
			ClearDragClass();
		}

		private Task OpenFilePickerAsync()
			=> fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

		private void OnInputFileChanged(InputFileChangeEventArgs e)
		{
			ClearDragClass();
			var files = e.GetMultipleFiles();
			foreach (var file in files)
			{
				fileNames.Add(file.Name);
				this.files.Add(file);
			}
		}

		private async Task Upload()
		{
			foreach (var file in files)
			{
				foreach (var zipFormat in zipFormats)
				{
					if (file.Name.Contains(zipFormat))
					{
						await ExtractModToDirectoryAsync(file, Path.Combine(UserConfigurationService.Current.GameDirectory, "Mods"));
					}
				}
			}
		}

		// TODO: Snackbar für Feedback und Clear Items
		private async Task ExtractModToDirectoryAsync(IBrowserFile file, string targetDirectory)
		{
			var extension = Path.GetExtension(file.Name).ToLowerInvariant();

			// Erstelle das Zielverzeichnis, falls es nicht existiert
			Directory.CreateDirectory(targetDirectory);

			// Datei in einen temporären Pfad speichern
			var tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + extension);
			using (var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 1024)) // 1 GB
			using (var fs = File.Create(tempFilePath))
			{
				await stream.CopyToAsync(fs);
			}

			if (extension == ".zip")
			{
				ZipFile.ExtractToDirectory(tempFilePath, targetDirectory, overwriteFiles: true);
			}
			else if (extension == ".rar" || extension == ".7z" || extension == ".tar" || extension == ".gzip")
			{
				using (var archive = ArchiveFactory.Open(tempFilePath))
				{
					foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
					{
						entry.WriteToDirectory(targetDirectory, new ExtractionOptions()
						{
							ExtractFullPath = true,
							Overwrite = true
						});
					}
				}
			}
			else
			{
				throw new NotSupportedException($"Format {extension} wird nicht unterstützt.");
			}

			// Optional: temporäre Datei löschen
			File.Delete(tempFilePath);
			await ClearAsync();

			Snackbar.Add("Mod successfully installed", Severity.Success);
		}

		private void SetDragClass()
				=> dragClass = $"{DefaultDragClass} mud-border-primary";

		private void ClearDragClass()
			=> dragClass = DefaultDragClass;
	}
}
