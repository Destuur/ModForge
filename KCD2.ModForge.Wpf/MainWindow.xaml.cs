using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Mods;
using KCD2.ModForge.Shared.Services;
using KCD2.ModForge.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System.Windows;
using System.Windows.Input;

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
			serviceCollection.AddSingleton<IFolderPickerService, FolderPickerService>();
			serviceCollection.AddSingleton<UserConfigurationService>();
			serviceCollection.AddSingleton<OrchestrationService>();
			serviceCollection.AddSingleton<ModService>();
			serviceCollection.AddSingleton<LocalizationService>();
			serviceCollection.AddSingleton<IconService>();
			serviceCollection.AddSingleton<PerkService>();
			serviceCollection.AddSingleton<ModCollection>();
			Resources.Add("services", serviceCollection.BuildServiceProvider());

			InitializeComponent();
		}

		private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}

		private void Minimize_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = this.WindowState == WindowState.Maximized
				? WindowState.Normal
				: WindowState.Maximized;
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}