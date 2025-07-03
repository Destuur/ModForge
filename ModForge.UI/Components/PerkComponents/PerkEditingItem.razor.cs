using ModForge.Shared.Factories;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ModForge.UI.Components.PerkComponents
{
	public partial class PerkEditingItem
	{
		private IEnumerable<IAttribute> sortedAttributes => EditingPerk.Attributes.OrderBy(x => x.Value.GetType().Name).ToList();
		private List<IAttribute> filteredAttributes = new();
		private bool isOpen;

		[Inject]
		public ModService? ModService { get; set; }
		[Parameter]
		public Perk EditingPerk { get; set; }
		[Parameter]
		public Perk OriginalPerk { get; set; }
		public IList<IAttribute> Attributes { get; private set; }

		public void ResetBuff()
		{
			//EditingPerk = Perk.GetDeepCopy(OriginalPerk);
			StateHasChanged();
		}

		public void Remove(string attribute)
		{
			EditingPerk.Attributes = EditingPerk.Attributes.Where(attr => attr?.Name != attribute).ToList();

			StateHasChanged();
			UpdateFilteredAttributes();
		}

		public void AddAttribute(IAttribute attribute)
		{
			if (EditingPerk.Attributes.Any(x => string.Equals(x.Name, attribute.Name, StringComparison.Ordinal)))
			{
				return;
			}
			if (EditingPerk.Attributes.Any(x => x.Name.Equals(attribute.Name, StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			if (!EditingPerk.Attributes.Contains(attribute))
			{
				EditingPerk.Attributes.Add(attribute);
			}
			UpdateFilteredAttributes();
			StateHasChanged();
		}

		private void UpdateFilteredAttributes()
		{
			filteredAttributes = Attributes
				.Where(attribute =>
					!attribute.Name.Contains("buff") &&
					!attribute.Name.Contains("duration") &&
					!attribute.Name.Contains("implementation") &&
					!EditingPerk.Attributes.Any(x => x.Name == attribute.Name))
				.ToList();
			StateHasChanged();
		}

		public void ToggleDrawer()
		{
			isOpen = !isOpen;
		}

		private string FormatName(string raw)
		{
			if (string.IsNullOrWhiteSpace(raw))
				return string.Empty;

			// Unterstriche durch Leerzeichen ersetzen
			string noUnderscores = raw.Replace("_", " ");

			// CamelCase trennen
			string withSpaces = Regex.Replace(noUnderscores, "(?<!^)([A-Z])", " $1");

			// Jeden Wortanfang großschreiben
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(withSpaces.ToLower());
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			Attributes = AttributeFactory.GetAllAttributes();
			//OriginalPerk = Perk.GetDeepCopy(OriginalPerk);
			UpdateFilteredAttributes();
		}

	}
}
