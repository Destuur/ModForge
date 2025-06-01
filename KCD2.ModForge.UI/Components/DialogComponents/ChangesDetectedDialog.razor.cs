using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Components.DialogsComponents
{
	public partial class ChangesDetectedDialog
	{
		[CascadingParameter]
		private IMudDialogInstance MudDialog { get; set; }

		[Parameter]
		public string ContentText { get; set; }

		[Parameter]
		public string ButtonText { get; set; }

		private void Submit() => MudDialog.Close(DialogResult.Ok(true));

		private void Cancel() => MudDialog.Cancel();
	}
}
