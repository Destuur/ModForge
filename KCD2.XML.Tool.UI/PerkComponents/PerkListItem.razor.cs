using KCD2.XML.Tool.Shared.Models;
using KCD2.XML.Tool.Shared.Mods;
using KCD2.XML.Tool.Shared.Services;
using KCD2.XML.Tool.UI.AttributeComponents;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.UI.PerkComponents
{
	public partial class PerkListItem
	{
		private ModDescription? mod;
		private string descAttribute = "perk_ui_desc";
		private string loreDescAttribute = "perk_ui_lore_desc";
		private string nameAttribute = "perk_ui_name";

		[Inject]
		public ModService? ModService { get; set; }
		[Inject]
		public LocalizationService? LocalizationService { get; set; }
		[Parameter]
		public Perk? Perk { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();
			mod = ModService!.GetMod();
		}
	}
}
