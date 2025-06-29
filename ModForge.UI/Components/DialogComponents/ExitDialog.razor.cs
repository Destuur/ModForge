using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ModForge.UI.Components.DialogComponents
{
	public partial class ExitDialog
	{
		[CascadingParameter]
		private IMudDialogInstance MudDialog { get; set; }
		[Parameter]
		public string ContentText { get; set; }
		[Parameter]
		public string CancelButton { get; set; }
		[Parameter]
		public string SaveAndExitButton { get; set; }
		[Parameter]
		public string ExitButton { get; set; }

		private void SaveAndExit() => MudDialog.Close(DialogResult.Ok(true));

		private void Exit() => MudDialog.Close(DialogResult.Ok(false));

		private void Cancel() => MudDialog.Cancel();
	}
}
