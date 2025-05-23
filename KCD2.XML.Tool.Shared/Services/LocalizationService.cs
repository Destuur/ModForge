using KCD2.XML.Tool.Shared.Models;
using KCD2.XML.Tool.Shared.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Shared.Services
{
	public class LocalizationService
	{
		private List<Localization> enLocalizations = new();
		private List<Localization> deLocalizations = new();

		public void AddLocalization(Localization localization)
		{
			deLocalizations.Add(localization);
		}

		public Localization GetLocalization(string attribute, string id)
		{
			var localization = deLocalizations.FirstOrDefault(x => x.Id == id);
			localization.Attribute = attribute;
			return localization;
		}
	}
}
