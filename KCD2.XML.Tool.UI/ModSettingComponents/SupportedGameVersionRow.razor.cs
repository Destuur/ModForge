using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Text.RegularExpressions;
using static MudBlazor.Colors;

namespace KCD2.XML.Tool.UI.ModSettingComponents
{
	public partial class SupportedGameVersionRow
	{
		private string? supportedGameVersion = "0.0.0";
		private bool isNotSupported = true;

		[Inject]
		public ModService? ModService { get; set; }

		public void SaveMod()
		{
			if (ModService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(supportedGameVersion))
			{
				supportedGameVersion = string.Empty;
				return;
			}

			if (ModService.GetAllSupportedVersions().Contains(supportedGameVersion))
			{
				return;
			}

			if (supportedGameVersion == "0.0.0")
			{
				return;
			}

			if (isNotSupported)
			{
				return;
			}

			ModService.AddSupportedVersion(supportedGameVersion!);
		}

		public void AddVersion()
		{
			if (ModService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(supportedGameVersion))
			{
				supportedGameVersion = string.Empty;
				return;
			}

			if (ModService.GetAllSupportedVersions().Contains(supportedGameVersion))
			{
				supportedGameVersion = string.Empty;
				return;
			}

			if (supportedGameVersion == "0.0.0")
			{
				supportedGameVersion = string.Empty;
				return;
			}

			var regex = new Regex(@"^\d+\.\d+(\.\d+|\*)$");

			if (!regex.IsMatch(supportedGameVersion))
			{
				supportedGameVersion = string.Empty;
				return;
			}

			ModService.AddSupportedVersion(supportedGameVersion!);
			supportedGameVersion = "0.0.0";
		}

		public void RemoveVersion(string modVersion)
		{
			if (ModService is null)
			{
				return;
			}

			ModService.RemoveSupportedVersion(modVersion);
		}

		// TODO: Versionsnummer kann mit 0.0.0134124 eingegeben werden.
		public string ValidateVersion(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return "Version darf nicht leer sein. Erlaubt sind: 1.2.3 oder 1.2*";

			// Erlaubt entweder:
			// - 1.2.3   → drei Zahlen mit Punkten
			// - 1.2*    → zwei Zahlen mit Punkt und danach ein Stern
			var regex = new Regex(@"^\d+\.\d+(\.\d+|\*)$");

			if (!regex.IsMatch(value))
				return "Ungültiges Format. Erlaubt sind: 1.2.3 oder 1.2*";

			return string.Empty;
		}
	}
}
