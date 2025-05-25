using KCD2.XML.Tool.Shared.Adapter;
using KCD2.XML.Tool.Shared.Mods;
using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.UI.Pages
{
	public partial class Modify
	{

		[Inject]
		public ModService? Service { get; init; }
		[Inject]
		public IXmlAdapter? Adapter { get; init; }
		public IEnumerable<IModItem>? ModItems { get; set; }

		public void WriteXml()
		{
			if (Adapter is null)
			{
				return;
			}

			if (ModItems is null)
			{
				return;
			}

			//Adapter.WriteModItems(ModItems);
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (Service is not null)
			{
				ModItems = Service.GetAll().ToList();
			}
		}
	}
}
