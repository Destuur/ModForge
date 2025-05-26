using KCD2.ModForge.Shared.Mods;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Components.ModCollectionComponents
{
	public partial class ModListItem
	{
		[Parameter]
		public ModDescription? Mod { get; set; }
	}
}
