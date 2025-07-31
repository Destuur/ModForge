using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.STORM;
using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Selectors;
using ModForge.Shared.Services;
using System.Globalization;

namespace ModForge.UI.Pages
{
	public partial class StormItems
	{
		private List<StormDto> stormDtos;
		private bool isLoaded;

		[Inject]
		private NavigationManager? NavigationManager { get; set; }
		[Inject]
		public IStringLocalizer<StormItems>? Localizer { get; set; }
		[Inject]
		public StormService? StormService { get; set; }
		[Inject]
		public IStringLocalizer<MessageService>? L { get; set; }
		public string? SearchStorm { get; set; }
		public StormDto? SelectedStorm { get; set; }
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

		private void NavigateToItem(string id)
		{
			NavigationManager.NavigateTo($"/stormfile/{id}");
		}

		private void SearchStormFiles()
		{
			if (StormService == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(SearchStorm))
			{
				stormDtos = StormService.GetStormDtos();
				return;
			}
			stormDtos = StormService.GetStormDtos().Where(x => x.DataPoint.Endpoint.ToLower().Contains(SearchStorm.ToLower())).ToList();
		}

		private void FilterStormFiles(string category)
		{
			if (string.IsNullOrWhiteSpace(category))
			{
				return;
			}
			stormDtos = category == "Miscellaneous" ? StormService.GetStormDtos().FindAll(x => x.Category is null) : StormService.GetStormDtos().FindAll(x => x.Category == category);
		}

		private void SelectStorm(StormDto storm)
		{
			if (storm == null) return;

			SelectedStorm = storm;
		}

		protected override void OnInitialized()
		{
			isLoaded = false;
			if (StormService is null)
			{
				return;
			}
			SetLanguage();
			stormDtos = StormService.GetStormDtos();
			var operationCategories = OperationParser.Categories;
			var selectors = SelectorParser.SelectorAttributes;
			isLoaded = true;
		}
	}
}