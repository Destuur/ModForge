using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ModForge.Localizations;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Services;
using ModForge.UI.Components.MenuComponents;
using ModForge.UI.Pages;
using MudBlazor;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class Perks
	{
		private List<IModItem> perks;
		private bool _isLoaded = false;

		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Parameter]
		public string ModId { get; set; }
		[Parameter]
		public EventCallback ToggledDrawer { get; set; }
		[Inject]
		public IJSRuntime JSRuntime { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public ILogger<Loadouts> Logger { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }
		[Inject]
		public XmlService XmlService { get; set; }
		[Inject]
		public LocalizationService LocalizationService { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		[Inject]
		public IconService IconService { get; set; }
		public string SearchPerk { get; set; }
		public IModItem? SelectedModItem { get; set; }

		public async Task ToggleDrawer()
		{
			await ToggledDrawer.InvokeAsync();
		}

		private string GetModItemId(IModItem modItem)
		{
			if (SelectedModItem is null)
			{
				return null;
			}

			var foundAttribute = modItem.Attributes.FirstOrDefault(x => x.Name == "metaperk_id");

			if (foundAttribute is null)
			{
				return $"#player.soul:AddPerk('{SelectedModItem.Id}')";
			}

			return $"#player.soul:AddPerk('{foundAttribute.Value.ToString()}')";
		}

		public void FilterPerks(string skill)
		{
			if (XmlService is null)
			{
				return;
			}

			SearchPerk = string.Empty;

			var filtered = XmlService.Perks
				.Where(x => x.Attributes.Any(attr =>
					string.Equals(attr.Value.ToString(), skill, StringComparison.OrdinalIgnoreCase)));

			if (!filtered.Any())
			{
				filtered = XmlService.Perks
					.Where(x => !x.Attributes.Any(attr =>
						string.Equals(attr.Name, "skill_selector", StringComparison.OrdinalIgnoreCase)));
			}

			perks = filtered.ToList();
		}

		private void SelectModItem(IModItem modItem)
		{
			if (modItem is null)
			{
				return;
			}

			SelectedModItem = modItem;
			StateHasChanged();
		}

		public void SearchPerks()
		{
			if (XmlService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(SearchPerk))
			{
				perks = XmlService.Perks.ToList();
				return;
			}

			var filtered = XmlService.Perks.Where(x => LocalizationService.GetName(x) is not null &&
														LocalizationService.GetName(x)!.ToLower().Contains(SearchPerk.ToLower()) ||
														x.Attributes.FirstOrDefault(x => x.Name.ToLower().Contains("name")).Value.ToString().ToLower().Contains(SearchPerk.ToLower()));


			perks = filtered.ToList();
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

			var description = LocalizationService.GetDescription(modItem);

			if (description is null)
			{
				return "Description not found";
			}

			return description;
		}

		private string GetSkillSelector(IModItem modItem)
		{
			var attribute = modItem.Attributes.FirstOrDefault(x => x.Name == "skill_selector");

			if (attribute is null)
			{
				var name = modItem.Attributes.FirstOrDefault(x => x.Name.Contains("name")).Value;
				return "Miscellaneous";
			}

			return $"{attribute.Value.ToString()}";
		}

		private string GetLevel(IModItem modItem)
		{
			var attribute = modItem.Attributes.FirstOrDefault(x => x.Name == "level");

			if (attribute is null)
			{
				return "System Perk";
			}

			return $"Lvl {attribute.Value.ToString()}";
		}

		protected override async Task OnInitializedAsync()
		{
			if (ModService.TryGetModFromCollection(ModId) == false)
			{
				ModService.ClearCurrentMod();
			}
			perks = await Task.Run(() => XmlService.Perks.ToList());
			_isLoaded = true;
		}
	}
}