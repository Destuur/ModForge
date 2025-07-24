using Microsoft.AspNetCore.Components;
using ModForge.Localizations;

namespace ModForge.UI.Components.Overlays
{
	public partial class AnimatedStatusText
	{
		private int currentIndex = Random.Shared.Next(0, 35);
		private string CurrentText => Messages.Length > 0 ? Messages[currentIndex] : "";
		private string textClass = "slide-in";

		[Parameter]
		public string[] Messages { get; set; } = [];
		[Parameter]
		public int IntervalMs { get; set; } = 1000;

		protected override async Task OnInitializedAsync()
		{
			while (true)
			{
				textClass = "slide-in";
				StateHasChanged();

				await Task.Delay(IntervalMs);
				textClass = "slide-out";
				StateHasChanged();

				await Task.Delay(500); // Zeit für Slide-out
				currentIndex = Random.Shared.Next(0, Messages.Count());
			}
		}
	}
}
