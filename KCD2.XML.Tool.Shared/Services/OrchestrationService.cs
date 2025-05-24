using KCD2.XML.Tool.Shared.Adapter;
using Microsoft.Extensions.Hosting;

namespace KCD2.XML.Tool.Shared.Services
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
