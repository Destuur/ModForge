using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Services;
using ModForge.UI.Components.MenuComponents;
using MudBlazor;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class Buffs
	{
		private List<IModItem> buffs;

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
		public string SearchBuff { get; set; }

		public async Task ToggleDrawer()
		{
			await ToggledDrawer.InvokeAsync();
		}

		public void FilterBuffs(string skill)
		{
			if (XmlService is null)
			{
				return;
			}

			SearchBuff = string.Empty;

			var filtered = XmlService.Buffs
				.Where(x => x.Attributes.Any(attr =>
					string.Equals(attr.Value.ToString(), skill, StringComparison.OrdinalIgnoreCase)));

			if (!filtered.Any())
			{
				filtered = XmlService.Buffs
					.Where(x => !x.Attributes.Any(attr =>
						string.Equals(attr.Name, "buff_class_id", StringComparison.OrdinalIgnoreCase)));
			}

			buffs = filtered.ToList();
		}

		public void SearchBuffs()
		{
			if (XmlService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(SearchBuff))
			{
				buffs = XmlService.Buffs.ToList();
				return;
			}

			string filter = SearchBuff;

			var filtered = XmlService.Buffs.Where(x => LocalizationService.GetName(x) is not null && LocalizationService.GetName(x).Contains(filter));


			buffs = filtered.ToList();
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

		private string GetBuffClass(IModItem modItem)
		{
			var attribute = modItem.Attributes.FirstOrDefault(x => x.Name == "buff_class_id");

			if (attribute is null)
			{
				var name = modItem.Attributes.FirstOrDefault(x => x.Name.Contains("name")).Value;
				return "Miscellaneous";
			}

			return $"{attribute.Value.ToString()}";
		}

		private string GetImplementation(IModItem modItem)
		{
			var attribute = modItem.Attributes.FirstOrDefault(x => x.Name == "implementation");

			if (attribute is null)
			{
				return "No Implementation";
			}

			return $"{attribute.Value.ToString()}";
		}

		public void NavigateToBuff(IModItem modItem)
		{
			if (NavigationManager is null)
			{
				return;
			}
			NavigationManager.NavigateTo($"editing/moditem/{modItem.Id}");
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			buffs = XmlService.Buffs.ToList();
		}
	}
}