using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ModForge.UI.Components.DialogComponents
{
	public partial class TwoButtonExitDialog
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

		private void Exit() => MudDialog.Close(DialogResult.Ok(true));

		private void Cancel() => MudDialog.Cancel();
	}
}