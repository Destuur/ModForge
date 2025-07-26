using MudBlazor;

namespace ModForge.UI.Themes
{
	public static class ModForgeThemes
	{
		public static MudTheme DarkTheme = new MudTheme()
		{
			PaletteLight = new PaletteLight()
			{
				Primary = "#df6f00",
				Secondary = "#f48fb1",
				Background = "#00000000", // durchsichtig, damit CSS durchscheint
				Surface = "#2e2e2e",
				AppbarBackground = "#1a1a2e",
				DrawerBackground = "#1e1e1e",
				DrawerIcon = "#c1c1c4",
				DrawerText = "#c1c1c4",
				TextPrimary = "#c1c1c4",
				GrayDefault = "#000000",
				ActionDisabled = "#7a7a7d",
				ActionDefault = "#ffffff",
				TextDisabled = "#7a7a7d",
				ActionDisabledBackground = "#a14b0044",
				BackgroundGray = "#000000",
				Dark = "#ffffff",
				DarkContrastText = "#ffffff",
				DarkDarken = "#ffffff",
				DarkLighten = "#000000",
				LinesInputs = "#c1c1c44d",
				TextSecondary = "#99999c",
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
				AppbarHeight = "0",
			},
			Typography = new Typography()
			{
				Default = new DefaultTypography()
				{
					FontFamily = new[] { "Rubik" }
				}
			}
		};
	}
}
