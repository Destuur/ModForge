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
	public partial class BuffEditingPage
	{
		private Buff buffCopy;

		[Parameter]
		public string Id { get; set; }
		[Inject]
		public XmlToJsonService XmlToJsonService { get; set; }
		[Inject]
		public NavigationService NavigationService { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();
			if (XmlToJsonService is null)
			{
				return;
			}

			var tempBuff = XmlToJsonService.Buffs!.FirstOrDefault(x => x.Id == Id)!;
			buffCopy = Buff.GetDeepCopy(tempBuff);
		}
	}
}
