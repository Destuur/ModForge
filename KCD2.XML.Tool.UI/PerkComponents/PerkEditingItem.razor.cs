using KCD2.XML.Tool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.UI.PerkComponents
{
	public partial class PerkEditingItem
	{
		[Parameter]
		public Perk Perk { get; set; }
	}
}
