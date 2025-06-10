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
				Primary = "#a14b00",
				Secondary = "#f48fb1",
				Background = "#00000000", // durchsichtig, damit CSS durchscheint
				Surface = "#00000000",
				AppbarBackground = "#1a1a2e",
				DrawerBackground = "#1a1a2e",
				TextPrimary = "#c1c1c4",
				GrayDefault = "#d9d9d9"
			},
			PaletteDark = new PaletteDark()
			{
				Primary = "#90caf9",
				Secondary = "#f48fb1",
				Background = "#00000000", // durchsichtig, damit CSS durchscheint
				Surface = "#00000000",
				AppbarBackground = "#1a1a2e",
				DrawerBackground = "#1a1a2e",
				TextPrimary = "#c1c1c4",
			},
			LayoutProperties = new LayoutProperties()
			{
				DefaultBorderRadius = "6px"
			}
		};
	}
}
