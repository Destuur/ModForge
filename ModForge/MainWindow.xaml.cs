using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModForge.Localizations;
using ModForge.Services;
using ModForge.Shared.Configurations;
using ModForge.Shared.Services;
using MudBlazor.Services;
using Serilog;
using System.Windows;
using System.Windows.Input;

namespace ModForge
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			var logger = new LoggerConfiguration()
			.WriteTo.Console()
			.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
			.CreateLogger();

			var serviceCollection = new ServiceCollection();

			serviceCollection.AddLogging(configure =>
			{
				configure.ClearProviders();
				configure.AddSerilog(logger, dispose: true);
			});

			serviceCollection.AddWpfBlazorWebView();
			serviceCollection.AddMudServices()
							 .AddModForgeServices()
							 .AddModForgeAdapters()
							 .AddSingleton<IFolderPickerService, FolderPickerService>();

			serviceCollection.AddLocalization();
			serviceCollection.AddTransient<MessageService>();

#if DEBUG
			serviceCollection.AddBlazorWebViewDeveloperTools();
#endif

			var serviceProvider = serviceCollection.BuildServiceProvider();

			serviceProvider.AddBuildHandler();
			Resources.Add("services", serviceProvider);

			InitializeComponent();
		}

		#region WPF Window Methods
		private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				if (e.ClickCount == 2)
				{
					// Doppelklick: Maximieren oder wiederherstellen
					this.WindowState = this.WindowState == WindowState.Maximized
						? WindowState.Normal
						: WindowState.Maximized;
				}
				else
				{
					// Einfaches DragMove für Fenster verschieben
					this.DragMove();
				}
			}
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
		#endregion
	}
}