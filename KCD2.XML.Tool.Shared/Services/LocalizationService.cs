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

		public bool IsFilled(Language language)
		{
			switch (language)
			{
				case Language.NotValid:
					return false;
					break;
				case Language.Chineses: 
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Chineset:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Czech:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.English:
					return enLocalizations.Count > 0 ? true : false;
					break;
				case Language.French:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.German:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Italian:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Japanese:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Korean:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Polish:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Portuguese:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Russian:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Spanish:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Turkish:
					return deLocalizations.Count > 0 ? true : false;
					break;
				case Language.Ukrainian:
					return deLocalizations.Count > 0 ? true : false;
					break;
				default: return false;
					break;
			}
		}
	}
}
