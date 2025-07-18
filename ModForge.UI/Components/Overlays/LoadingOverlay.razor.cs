using Microsoft.AspNetCore.Components;

namespace ModForge.UI.Components.Overlays
{
	public partial class LoadingOverlay
	{
		[Parameter] public bool IsVisible { get; set; }

		private string[] Messages = new[]
		{
			"Read XML Files…",
			"Skill Perks…",
			"Apply Buffs…",
			"Yank Pizzles…",
			"Save Data…"
		};
	}
}
