using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Components.Pages
{
	public partial class Home
	{
		[Inject]
		public OrchestrationService OrchestrationService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await OrchestrationService.Initialize();
		}
	}
}
