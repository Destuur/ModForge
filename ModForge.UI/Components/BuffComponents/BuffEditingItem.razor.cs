using ModForge.Shared.Factories;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ModForge.UI.Components.BuffComponents
{
	public partial class BuffEditingItem
	{
		private IEnumerable<IAttribute> sortedAttributes => EditingBuff.Attributes.OrderBy(x => x.Value.GetType().Name).ToList();
		private List<IAttribute> filteredAttributes = new();
		private bool isOpen;


		[Inject]
		public ModService? ModService { get; set; }
		[Parameter]
		public Buff EditingBuff { get; set; }
		[Parameter]
		public Buff OriginalBuff { get; set; }
		public IList<IAttribute> Attributes { get; private set; }


		public void ResetBuff()
		{
			//EditingBuff = Buff.GetDeepCopy(OriginalBuff);
			StateHasChanged();
		}

		public void Remove(string attribute)
		{
			EditingBuff.Attributes = EditingBuff.Attributes.Where(attr => attr?.Name != attribute).ToList();

			StateHasChanged();
			UpdateFilteredAttributes();
		}

		public void AddAttribute(IAttribute attribute)
		{
			if (EditingBuff.Attributes.Any(x => string.Equals(x.Name, attribute.Name, StringComparison.Ordinal)))
			{
				return;
			}
			if (EditingBuff.Attributes.Any(x => x.Name.Equals(attribute.Name, StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			if (!EditingBuff.Attributes.Contains(attribute))
			{
				EditingBuff.Attributes.Add(attribute);
			}
			UpdateFilteredAttributes();
			StateHasChanged();
		}

		private void UpdateFilteredAttributes()
		{
			filteredAttributes = Attributes
				.Where(attribute =>
					!attribute.Name.Contains("perk") &&
					!attribute.Name.Contains("skill") &&
					!attribute.Name.Contains("stat") &&
					!EditingBuff.Attributes.Any(x => x.Name == attribute.Name))
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
			//OriginalBuff = Buff.GetDeepCopy(OriginalBuff);
			UpdateFilteredAttributes();
		}
	}
}
