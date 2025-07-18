using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.Attributes;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace ModForge.UI.Components.DialogComponents
{
	public partial class BuffParamDialog
	{
		private string searchString = string.Empty;
		private int selectedRowNumber = -1;
		private MudTable<string> mudTable;

		[CascadingParameter]
		private IMudDialogInstance? MudDialog { get; set; }
		[Parameter]
		public string? Title { get; set; }
		[Parameter]
		public string? ButtonText { get; set; }
		[Parameter]
		public string? CancleText { get; set; }
		public string? SelectedBuffParamKey { get; set; }

		private void Submit() => MudDialog!.Close(DialogResult.Ok(SelectedBuffParamKey));
		private void Cancel() => MudDialog!.Cancel();

		private string SelectedRowClassFunc(string element, int rowNumber)
		{
			if (selectedRowNumber == rowNumber)
			{
				selectedRowNumber = -1;
				return string.Empty;
			}
			else if (mudTable.SelectedItem != null && mudTable.SelectedItem.Equals(element))
			{
				selectedRowNumber = rowNumber;
				return "selected";
			}
			else
			{
				return string.Empty;
			}
		}

		private bool FilterFunc(string key)
		{
			if (string.IsNullOrWhiteSpace(searchString))
				return true;
			if (key.Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (BuffParamSerializer.GetName(key).Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (BuffParamSerializer.GetDescription(key).Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			return false;
		}
	}
}