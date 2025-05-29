using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Components.Overlays
{
	public partial class AnimatedStatusText
	{
		private int currentIndex = 0;
		private string CurrentText => Messages.Length > 0 ? Messages[currentIndex] : "";
		private string textClass = "slide-in";

		[Parameter]
		public string[] Messages { get; set; } = [];
		[Parameter]
		public int IntervalMs { get; set; } = 2000;

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
				currentIndex = (currentIndex + 1) % Messages.Length;
			}
		}
	}
}
