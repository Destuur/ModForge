using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.STORM;
using ModForge.Shared.Services;

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
			stormDtos = StormService.GetStormDtos();
			var operationCategories = OperationParser.Categories;
			var selectors = SelectorParser.SelectorAttributes;
			isLoaded = true;
		}
	}
}