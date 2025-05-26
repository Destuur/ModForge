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
