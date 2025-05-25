using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.UI.Components.ModSettingComponents
{
	public partial class ModSettingForm
	{
		private string name = string.Empty;
		private string description = string.Empty;
		private string author = string.Empty;
		private string version = "1.0";
		private DateTime createdOn = DateTime.Now.Date;
		private string modId = string.Empty;
		private bool modifiesLevel;

		private SupportedGameVersionRow? supportedGameVersionRow;

		public ModSettingForm()
		{
			var currentTime = DateTime.Now;
			createdOn = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
		}

		[Inject]
		public ModService? ModService { get; set; }

		public void GetModId()
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}

			var modIdStrings = name.Trim().ToLower().Split(' ');
			modId = string.Join('_', modIdStrings);
			StateHasChanged();
		}

		public async Task SaveMod()
		{
			if (string.IsNullOrEmpty(name) ||
				string.IsNullOrEmpty(modId) ||
				ModService is null)
			{
				return;
			}

			if (supportedGameVersionRow is null)
			{
				return;
			}

			supportedGameVersionRow.SaveMod();
			await ModService.SaveMod(name, description, author, version, createdOn, modId, modifiesLevel);
		}

		private string ValidateModName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return "Der Name darf nicht leer sein.";

			// Nur Buchstaben und Leerzeichen erlaubt
			var regex = new Regex(@"^[a-zA-Z\s]+$");

			if (!regex.IsMatch(value))
				return "Der Name darf nur Buchstaben und Leerzeichen enthalten. Keine Zahlen oder Sonderzeichen.";

			return null;
		}
	}
}
