using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModForge.Localizations;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Services;
using ModForge.UI.Components.MenuComponents;
using MudBlazor;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class Armors
	{
		private List<IModItem> armors;
		private bool _isLoaded = false;

		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Parameter]
		public EventCallback ToggledDrawer { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public ILogger<Loadouts> Logger { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public XmlService XmlService { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public LocalizationService LocalizationService { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		public string SearchArmor { get; set; }
		public IModItem? SelectedModItem { get; set; }

		private void SelectModItem(IModItem modItem)
		{
			if (modItem is null)
			{
				return;
			}

			SelectedModItem = modItem;
			StateHasChanged();
		}

		public async Task ToggleDrawer()
		{
			await ToggledDrawer.InvokeAsync();
		}

		public void FilterArmors(string skill)
		{
			if (XmlService is null)
			{
				return;
			}

			SearchArmor = string.Empty;

			var filtered = XmlService.Armors
				.Where(x => x.Attributes.Any(attr =>
					string.Equals(attr.Value.ToString(), skill, StringComparison.OrdinalIgnoreCase)));

			if (!filtered.Any())
			{
				filtered = XmlService.Armors
					.Where(x => !x.Attributes.Any(attr =>
						string.Equals(attr.Name, "skill_selector", StringComparison.OrdinalIgnoreCase)));
			}

			armors = filtered.ToList();
		}

		public void SearchArmors()
		{
			if (XmlService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(SearchArmor))
			{
				armors = XmlService.Armors.ToList();
				return;
			}

			var filtered = XmlService.Armors.Where(x => LocalizationService.GetName(x) is not null &&
														LocalizationService.GetName(x)!.ToLower().Contains(SearchArmor.ToLower()) ||
														x.Attributes.FirstOrDefault(x => x.Name.ToLower().Contains("name")).Value.ToString().ToLower().Contains(SearchArmor.ToLower()));


			armors = filtered.ToList();
		}

		private string GetName(IModItem modItem)
		{
			if (LocalizationService is null || modItem is null)
			{
				return string.Empty;
			}

			var name = LocalizationService.GetName(modItem);

			if (name is null)
			{
				return "Name not found";
			}

			return name;
		}

		private string GetLoreDescription(IModItem modItem)
		{
			if (LocalizationService is null || modItem is null)
			{
				return string.Empty;
			}

			var name = LocalizationService.GetLoreDescription(modItem);

			if (name is null)
			{
				return "Lore Description not found";
			}

			return name;
		}

		private string GetDescription(IModItem modItem)
		{
			if (LocalizationService is null || modItem is null)
			{
				return string.Empty;
			}

			var name = LocalizationService.GetDescription(modItem);

			if (name is null)
			{
				return "Description not found";
			}

			return name;
		}

		private string GetSkillSelector(IModItem modItem)
		{
			var attribute = modItem.Attributes.FirstOrDefault(x => x.Name == "skill_selector");

			if (attribute is null)
			{
				return "Miscellaneous";
			}

			return $"{attribute.Value.ToString()}";
		}

		private string GetLevel(IModItem modItem)
		{
			var attribute = modItem.Attributes.FirstOrDefault(x => x.Name == "Price");

			if (attribute is null)
			{
				return "-";
			}

			return $"Price: {(Double.TryParse(attribute.Value.ToString(), out var price) ? $"{price / 10} Groschen" : "-")}";
		}

		public void NavigateToArmor(IModItem modItem)
		{
			if (NavigationManager is null)
			{
				return;
			}
			NavigationManager.NavigateTo($"editing/moditem/{modItem.Id}");
		}


		protected override async Task OnInitializedAsync()
		{
			armors = await Task.Run(() => XmlService.Armors.ToList());
			_isLoaded = true;
		}
	}
}