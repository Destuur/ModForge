using KCD2.XML.Tool.Shared;
using KCD2.XML.Tool.Shared.Adapter;
using KCD2.XML.Tool.Shared.Mods;
using KCD2.XML.Tool.Shared.Services;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace KCD2.XML.Tool
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("19_Kingdom Come Display Medium.ttf", "Kingdom Come Display Medium");
					fonts.AddFont("25_Kingdom_Light.ttf", "Kingdom Light");
					fonts.AddFont("6_Kingdom Come Regular.ttf", "Kingdom Come Regular");
					fonts.AddFont("8_Warhorse Manuscript.ttf", "Warhorse Manuscript");
				});

			builder.Services.AddMauiBlazorWebView();
			builder.Services.AddMudServices();

			builder.Services.AddSingleton<IXmlAdapter, XmlAdapter>();
			builder.Services.AddSingleton<ModService>();
			builder.Services.AddSingleton<LocalizationService>();
			builder.Services.AddSingleton<ModCollection>();

			//			builder.ConfigureLifecycleEvents(events =>
			//			{
			//#if WINDOWS
			//				events.AddWindows(windows =>
			//				{
			//					windows.OnWindowCreated(window =>
			//					{
			//						var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
			//						var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
			//						var appWindow = AppWindow.GetFromWindowId(windowId);

			//						// Titelleiste und Fensterrahmen ausblenden
			//						appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
			//					});
			//				});
			//#endif
			//			});

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
