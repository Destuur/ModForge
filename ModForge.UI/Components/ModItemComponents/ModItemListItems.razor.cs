using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModForge.UI.Components.ModItemComponents
{
	public partial class ModItemListItems
	{
		[Parameter]
		public List<IModItem>? ModItems { get; set; }
		[Inject]
		public LocalizationService LocalizationService { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }

		private string GetName(IModItem modItem)
		{
			try
			{
				var lang = UserConfigurationService.Current.Language;
				var attribute = modItem.Attributes.FirstOrDefault(x => x.Name.Contains("ui_name"));

				if (attribute is null)
				{
					return "null";
				}

				var key = attribute.Value.ToString();
				return LocalizationService.GetName(lang, key);
			}
			catch (Exception e)
			{
				return "Test";
			}
		}

		private string GetLoreDescription(IModItem modItem)
		{
			try
			{
				var lang = UserConfigurationService.Current.Language;
				var attribute = modItem.Attributes.FirstOrDefault(x => x.Name.Contains("ui_lore_desc"));

				if (attribute is null)
				{
					return "null";
				}

				var key = attribute.Value.ToString();
				return LocalizationService.GetName(lang, key);
			}
			catch (Exception e)
			{
				return "Test";
			}

		}

		private string GetDescription(IModItem modItem)
		{
			try
			{
				var lang = UserConfigurationService.Current.Language;
				var attribute = modItem.Attributes.FirstOrDefault(x => x.Name.Contains("ui_desc"));

				if (attribute is null)
				{
					return "null";
				}

				var key = attribute.Value.ToString();
				return LocalizationService.GetName(lang, key);
			}
			catch (Exception e)
			{
				return "Test";
			}

		}

		private string GetSkillSelector(IModItem modItem)
		{
			var attribute = modItem.Attributes.FirstOrDefault(x => x.Name == "skill_selector");

			if (attribute is null)
			{
				var name = modItem.Attributes.FirstOrDefault(x => x.Name.Contains("name")).Value;
				return "n/a";
			}

			return $"{attribute.Value.ToString()}";
		}

		private string GetLevel(IModItem modItem)
		{
			var attribute = modItem.Attributes.FirstOrDefault(x => x.Name == "level");

			if (attribute is null)
			{
				return "n/a";
			}

			return $"Lvl {attribute.Value.ToString()}";
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
		}
	}
}
