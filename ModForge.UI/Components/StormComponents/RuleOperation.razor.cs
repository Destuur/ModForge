using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Selectors;
using ModForge.Shared.Services;

namespace ModForge.UI.Components.StormComponents
{
	public partial class RuleOperation
	{
		private string currentOperation;
		private string currentAttribute;

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

		private async Task<IEnumerable<string>> SearchAttributeValue(string value, CancellationToken token)
		{
			if (string.IsNullOrEmpty(currentOperation))
				return Enumerable.Empty<string>();

			if (!OperationParser.Categories.TryGetValue(OperationCategory.Name, out var category))
				return Enumerable.Empty<string>();

			if (!category.OperationAttributes.TryGetValue(currentOperation, out var attributeDict))
				return Enumerable.Empty<string>();

			if (!attributeDict.TryGetValue(currentAttribute, out var values))
				return Enumerable.Empty<string>();

			if (string.IsNullOrWhiteSpace(value))
				return values;

			return values
				.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
		}

		private Func<string, CancellationToken, Task<IEnumerable<string>>> CreateSearchFunc(string operationName, string attributeName)
		{
			return async (value, token) =>
			{
				if (!OperationParser.Categories.TryGetValue(OperationCategory.Name, out var category))
					return Enumerable.Empty<string>();

				if (!category.OperationAttributes.TryGetValue(operationName, out var attributeDict))
					return Enumerable.Empty<string>();

				if (!attributeDict.TryGetValue(attributeName, out var values))
					return Enumerable.Empty<string>();

				if (string.IsNullOrWhiteSpace(value))
					return values;

				return values.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
			};
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
			OperationCategory.OperationAttributes.TryGetValue(value, out Dictionary<string, HashSet<string>> attributes);
			if (attributes == null)
			{
				return;
			}
			foreach (var attribute in attributes)
			{
				operation.Attributes.Add(attribute.Key, "");
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