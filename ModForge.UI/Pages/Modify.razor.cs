using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Services;

namespace ModForge.UI.Pages
{
	public partial class Modify
	{

		[Inject]
		public ModService? Service { get; init; }
		public IEnumerable<IModItem>? ModItems { get; set; }

		public void WriteXml()
		{
			//Adapter.WriteModItems(ModItems);
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (Service is not null)
			{
				ModItems = Service.GetCurrentModItems().ToList();
			}
		}
	}
}
