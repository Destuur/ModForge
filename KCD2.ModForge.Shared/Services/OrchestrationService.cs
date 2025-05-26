using KCD2.ModForge.Shared.Adapter;

namespace KCD2.ModForge.Shared.Services
{
	public class OrchestrationService
	{
		private readonly IXmlAdapter xmlAdapter;

		public OrchestrationService(IXmlAdapter xmlAdapter)
		{
			this.xmlAdapter = xmlAdapter;
		}

		public async Task Initialize()
		{
			await xmlAdapter.Initialize();
		}
	}
}
