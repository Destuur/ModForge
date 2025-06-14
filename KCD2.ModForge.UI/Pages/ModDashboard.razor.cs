using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI.Pages
{
	public partial class ModDashboard
	{
		[Inject]
		public NavigationManager Navigation { get; set; }
	}
}
