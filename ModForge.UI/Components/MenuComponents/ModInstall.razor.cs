using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Services;
using MudBlazor;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.Globalization;
using System.IO.Compression;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModForge.UI.Components.MenuComponents
{
	public partial class ModInstall
	{
		private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
		private bool isLoading;
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

		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }

		private void SetLanguage()
		{
			var language = UserConfigurationService.Current.Language;
			var culture = string.IsNullOrEmpty(language) ? CultureInfo.CurrentCulture : new CultureInfo(UserConfigurationService.Current.Language);

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		protected override void OnInitialized()
		{
			SetLanguage();
		}

		public async Task BackToDashboard()
		{
			await ChangeChildContent.InvokeAsync(typeof(Dashboard));
		}

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
			fileNames.Clear();
			files.Clear(); // <--- Wichtig: Liste leeren!
			var newFiles = e.GetMultipleFiles();
			foreach (var file in newFiles)
			{
				fileNames.Add(file.Name);
				files.Add(file);
			}
		}

		private async Task Upload()
		{
			isLoading = true;
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
			await ClearAsync();
			isLoading = false;
		}

		// TODO: Snackbar für Feedback und Clear Items
		private async Task ExtractModToDirectoryAsync(IBrowserFile file, string targetDirectory)
		{
			var extension = Path.GetExtension(file.Name).ToLowerInvariant();
			Directory.CreateDirectory(targetDirectory);

			// Garantiert eindeutiger Temp-Dateiname
			var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + extension);

			try
			{
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

				Snackbar.Add("Mod successfully installed", Severity.Success);
			}
			catch (Exception ex)
			{
				Snackbar.Add($"Fehler beim Installieren: {ex.Message}", Severity.Error);
				throw; // Optional: oder nur loggen
			}
			finally
			{
				// Versuche, die temporäre Datei zu löschen, auch bei Fehlern
				try { if (File.Exists(tempFilePath)) File.Delete(tempFilePath); } catch { }
			}
		}

		private void SetDragClass()
				=> dragClass = $"{DefaultDragClass} mud-border-primary";

		private void ClearDragClass()
			=> dragClass = DefaultDragClass;
	}
}
