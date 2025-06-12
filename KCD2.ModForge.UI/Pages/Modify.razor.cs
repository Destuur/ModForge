using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Pages
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
