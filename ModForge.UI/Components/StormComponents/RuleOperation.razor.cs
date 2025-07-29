using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.STORM;
using ModForge.Shared.Services;

namespace ModForge.UI.Components.StormComponents
{
	public partial class RuleOperation
	{
		[Inject]
		public StormService Storm { get; set; }
		[Parameter]
		public OperationCategory OperationCategory { get; set; }
		[Parameter]
		public EventCallback<List<GenericOperation>> AddedOperations { get; set; }
		public List<GenericOperation> Operations { get; set; }

		private void RemoveSelector(GenericOperation operation)
		{
			if (operation is null)
			{
				return;
			}
			Operations.Remove(operation);
		}

		private async Task<IEnumerable<string>> SearchSelector(string value, CancellationToken token)
		{
			if (string.IsNullOrEmpty(value))
				return Storm.Selectors.Keys;
			return Storm.Selectors.Keys.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}