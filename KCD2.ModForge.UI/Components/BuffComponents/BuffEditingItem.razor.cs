using KCD2.ModForge.Shared.Factories;
using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KCD2.ModForge.UI.Components.BuffComponents
{
	public partial class BuffEditingItem
	{
		private IEnumerable<IAttribute> sortedAttributes => Buff.Attributes.OrderBy(x => (x.Value is IList<BuffParam> ? 1 : 0, x.Value.GetType().Name)).ToList();
		private List<IAttribute> filteredAttributes = new();
		private bool isOpen;
		private Buff originalBuff;

		[Inject]
		public ModService? ModService { get; set; }
		[Parameter]
		public Buff Buff { get; set; }
		public IList<IAttribute> Attributes { get; private set; }

		// TODO: Remove funktioniert noch nicht richtig.
		// Exception abfangen.
		public void ResetBuff()
		{
			Buff = KCD2.ModForge.Shared.Models.ModItems.Buff.GetDeepCopy(originalBuff);
			StateHasChanged();
		}

		public void Remove(string attribute)
		{
			Buff.Attributes = Buff.Attributes.Where(attr => attr?.Name != attribute).ToList();

			UpdateFilteredAttributes();
			StateHasChanged();
		}

		public void AddAttribute(IAttribute attribute)
		{
			if (Buff.Attributes.Any(x => string.Equals(x.Name, attribute.Name, StringComparison.Ordinal)))
			{
				return;
			}
			if (Buff.Attributes.Any(x => x.Name.Equals(attribute.Name, StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			if (!Buff.Attributes.Contains(attribute))
			{
				Buff.Attributes.Add(attribute);
			}
			UpdateFilteredAttributes();
			StateHasChanged();
		}

		private void UpdateFilteredAttributes()
		{
			filteredAttributes = Attributes
				.Where(attribute =>
					!attribute.Name.Contains("perk") &&
					!attribute.Name.Contains("level") &&
					!attribute.Name.Contains("stat") &&
					!attribute.Name.Contains("skill") &&
					!Buff.Attributes.Any(x => x.Name == attribute.Name))
				.ToList();
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
			originalBuff = KCD2.ModForge.Shared.Models.ModItems.Buff.GetDeepCopy(Buff);
			UpdateFilteredAttributes();
		}
	}
}
