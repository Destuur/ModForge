using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.UI.ModSettingComponents
{
	public partial class ModSettingForm
	{
		private string name = string.Empty;
		private string description = string.Empty;
		private string author = string.Empty;
		private string version = string.Empty;
		private DateTime createdOn = DateTime.Now;
		private string modId = string.Empty;
		private bool modifiesLevel;
		private List<string> supportsVersions = new();

		public void GetModId()
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}

			var modIdStrings = name.ToLower().Split(' ');
			modId = string.Join('_', modIdStrings);
			StateHasChanged();
		}

		private string ValidateVersion(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return "Version darf nicht leer sein.";

			// Erlaubt entweder:
			// - 1.2.3   → drei Zahlen mit Punkten
			// - 1.2*    → zwei Zahlen mit Punkt und danach ein Stern
			var regex = new Regex(@"^\d+\.\d+(\.\d+|\*)$");

			if (!regex.IsMatch(value))
				return "Ungültiges Format. Erlaubt sind: 1.2.3 oder 1.2*";

			return null;
		}
	}
}
