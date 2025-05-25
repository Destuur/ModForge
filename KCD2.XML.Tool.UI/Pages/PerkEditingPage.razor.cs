using KCD2.XML.Tool.Shared.Models;
using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.UI.Pages
{
	public partial class PerkEditingPage
	{
		private Perk perk;

		[Parameter]
		public string PerkId { get; set; }
		[Inject]
		public PerkService? PerkService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (PerkService is null)
			{
				return;
			}
			perk = PerkService.GetPerk(PerkId);
		}
	}
}
