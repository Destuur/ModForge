using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModForge.Localizations;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using MudBlazor;
using System.Diagnostics;

namespace ModForge.UI.Components.ModListComponents
{
	public partial class ModListItems
	{
		[Parameter]
		public EventCallback ModDeleted { get; set; }
		[Parameter]
		public ModCollection Mods { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public ILogger<ModListItems> Logger { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }

		private void EditMod(string modId)
		{
			if (string.IsNullOrEmpty(modId))
			{
				return;
			}

			NavigationManager.NavigateTo($"/modoverview/{modId}");
		}

		private void ExportMod(ModDescription mod)
		{
			try
			{
				ModService.ExportMod(mod);
				Snackbar.Add("Mod successfully exported", Severity.Success);
			}
			catch (Exception e)
			{
				Logger.LogError($"Mod '{mod.Name}' could not be exported");
				Snackbar.Add("An error occured. Mod could not be exported", Severity.Error);
			}
		}

		private void GoToDirectory(ModDescription mod)
		{
			var path = Path.Combine(UserConfigurationService.Current.GameDirectory, "Mods", mod.Id);

			if (string.IsNullOrEmpty(path))
			{
				Snackbar.Add("An error occured. Directory not accessible", Severity.Error);
				return;
			}

			try
			{
				Process.Start("explorer.exe", path);
			}
			catch (Exception e)
			{

				throw;
			}
		}

		private async Task DeleteMod(ModDescription mod)
		{
			var path = Path.Combine(UserConfigurationService.Current.GameDirectory, "Mods", mod.Id);

			if (string.IsNullOrEmpty(path))
			{
				Snackbar.Add("An error occured. Directory not accessible", Severity.Error);
				return;
			}

			try
			{
				Directory.Delete(path, true);
				ModService.InitiateModCollections();
				await ModDeleted.InvokeAsync();
				Snackbar.Add("Mod successfully deleted", Severity.Success);
			}
			catch (Exception e)
			{

				throw;
			}
			StateHasChanged();
		}
	}
}