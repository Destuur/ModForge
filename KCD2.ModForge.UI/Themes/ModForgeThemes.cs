using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Themes
{
	public static class ModForgeThemes
	{
		public static MudTheme DarkTheme = new MudTheme()
		{
			PaletteLight = new PaletteLight()
			{
				Primary = "#90caf9",
				Secondary = "#f48fb1",
				Background = "#00000000", // durchsichtig, damit CSS durchscheint
				Surface = "#252539",
				AppbarBackground = "#1a1a2e",
				DrawerBackground = "#1a1a2e",
				TextPrimary = "#ffffff",
			},
			PaletteDark = new PaletteDark()
			{
				Primary = "#90caf9",
				Secondary = "#f48fb1",
				Background = "#00000000", // durchsichtig, damit CSS durchscheint
				Surface = "#252539",
				AppbarBackground = "#1a1a2e",
				DrawerBackground = "#1a1a2e",
				TextPrimary = "#ffffff",
			},

			LayoutProperties = new LayoutProperties()
			{
				DefaultBorderRadius = "6px"
			}
		};
	}
}
