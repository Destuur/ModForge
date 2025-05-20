namespace KCD2.XML.Tool
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			return new Window(new MainPage()) { Title = "KCD2.XML.Tool" };
		}
	}
}
