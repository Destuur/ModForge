using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Services;
using ModForge.UI.Components.MenuComponents;
using MudBlazor;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class Items
	{
		private List<IModItem> items;

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
		public LocalizationService LocalizationService { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		public string SearchItem { get; set; }

		public async Task ToggleDrawer()
		{
			await ToggledDrawer.InvokeAsync();
		}

		public void FilterItems(string skill)
		{
			if (XmlService is null)
			{
				return;
			}

			SearchItem = string.Empty;

			var filtered = XmlService.Perks
				.Where(x => x.Attributes.Any(attr =>
					string.Equals(attr.Value.ToString(), skill, StringComparison.OrdinalIgnoreCase)));

			if (!filtered.Any())
			{
				filtered = XmlService.Perks
					.Where(x => !x.Attributes.Any(attr =>
						string.Equals(attr.Name, "skill_selector", StringComparison.OrdinalIgnoreCase)));
			}

			items = filtered.ToList();
		}

		public void SearchItems()
		{
			if (XmlService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(SearchItem))
			{
				items = XmlService.Perks.ToList();
				return;
			}

			string filter = SearchItem;

			var filtered = XmlService.Perks.Where(x => LocalizationService.GetName(x) is not null && LocalizationService.GetName(x).Contains(filter));


			items = filtered.ToList();
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

		public void NavigateToItem(IModItem modItem)
		{
			if (NavigationManager is null)
			{
				return;
			}
			NavigationManager.NavigateTo($"editing/item/{modItem.Id}");
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			items = XmlService.Items.ToList();
		}
	}
}