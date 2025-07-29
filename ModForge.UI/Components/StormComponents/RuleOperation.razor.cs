using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.STORM.Operations;
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
		public List<GenericOperation> Operations { get; set; } = [];

		private void RemoveOperation(GenericOperation operation)
		{
			if (operation is null)
			{
				return;
			}
			Operations.Remove(operation);
		}

		private async Task<IEnumerable<string>> SearchOperation(string value, CancellationToken token)
		{
			if (string.IsNullOrEmpty(value))
				return OperationCategory.OperationTypes;
			return OperationCategory.OperationTypes.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
		}

		private void AddOperation()
		{
			Operations.Add(new GenericOperation());
		}

		private void GetOperationAttributes(string value, GenericOperation operation)
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			if (operation == null)
			{
				return;
			}
			OperationCategory.OperationAttributes.TryGetValue(value, out HashSet<string> attributes);
			if (attributes == null)
			{
				return;
			}
			foreach (var attribute in attributes)
			{
				operation.Attributes.Add(attribute, "");
			}
			operation.Name = value;
			StateHasChanged();
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
		}
	}
}