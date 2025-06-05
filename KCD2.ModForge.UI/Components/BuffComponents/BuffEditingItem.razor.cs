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
		private IEnumerable<IAttribute> sortedAttributes => Buff.Attributes.OrderBy(x => (x.Value is IList<BuffParam> ? 1 : 0, x.Value.GetType().Name));
		private IEnumerable<IAttribute> selectableAttributes => FilterAttributes();
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
			var tempList = new List<IAttribute>();

			foreach (var removeItem in Buff.Attributes)
			{
				if (removeItem.Name.Equals(attribute))
				{
					continue;
				}
				if (removeItem is not null)
				{
					tempList.Add(removeItem);
				}
			}

			Buff.Attributes = tempList.ToList();
			StateHasChanged();
		}

		public void AddAttribute(IAttribute attribute)
		{
			if (Buff.Attributes.Any(x => x.Name == attribute.Name))
			{
				return;
			}
			Buff.Attributes.Add(attribute);
			StateHasChanged();
		}

		private IEnumerable<IAttribute> FilterAttributes()
		{
			var tempList = new List<IAttribute>();

			foreach (var attribute in Attributes)
			{
				if (attribute.Name.Contains("perk") ||
					attribute.Name.Contains("level") ||
					attribute.Name.Contains("stat") ||
					attribute.Name.Contains("skill"))
				{
					continue;
				}
				if (Buff.Attributes.FirstOrDefault(x => x.Name == attribute.Name) == null)
				{
					tempList.Add(attribute);
				}
			}

			return tempList;
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
		}
	}
}
