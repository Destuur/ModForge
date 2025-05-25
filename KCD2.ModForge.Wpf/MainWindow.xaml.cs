using KCD2.XML.Tool.Shared.Adapter;
using KCD2.XML.Tool.Shared.Mods;
using KCD2.XML.Tool.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System.Windows;

namespace KCD2.ModForge.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddWpfBlazorWebView();
			serviceCollection.AddMudServices();
			serviceCollection.AddSingleton<IXmlAdapter, XmlAdapter>();
			serviceCollection.AddSingleton<OrchestrationService>();
			serviceCollection.AddSingleton<ModService>();
			serviceCollection.AddSingleton<LocalizationService>();
			serviceCollection.AddSingleton<IconService>();
			serviceCollection.AddSingleton<PerkService>();
			serviceCollection.AddSingleton<ModCollection>();
			Resources.Add("services", serviceCollection.BuildServiceProvider());

			InitializeComponent();
		}
	}
}