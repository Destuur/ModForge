using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ModForge.UI.Components.DialogComponents
{
	public partial class EntryDialog
	{
		[CascadingParameter]
		private IMudDialogInstance MudDialog { get; set; }

		[Parameter]
		public string ContentText { get; set; }

		[Parameter]
		public string ButtonText { get; set; }

		private void Submit() => MudDialog.Close(DialogResult.Ok(true));
	}
}

