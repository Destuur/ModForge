using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ModForge.Localizations;
using MudBlazor;

namespace ModForge.UI.Components.DialogComponents
{
	public partial class EntryDialog
	{
		[CascadingParameter]
		private IMudDialogInstance MudDialog { get; set; }

		[Parameter]
		public string ContentText { get; set; }
		[Inject]
		public IStringLocalizer<MessageService> L { get; set; }

		[Parameter]
		public string ButtonText { get; set; }

		private void Submit() => MudDialog.Close(DialogResult.Ok(true));
	}
}

