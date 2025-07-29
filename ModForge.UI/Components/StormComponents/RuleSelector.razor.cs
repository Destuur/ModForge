using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.STORM.Selectors;
using ModForge.Shared.Services;
using static MudBlazor.Colors;

namespace ModForge.UI.Components.StormComponents
{
	public partial class RuleSelector
	{
		[Inject]
		public StormService Storm { get; set; }
		[Parameter]
		public GenericSelector Selector { get; set; }
		[Parameter]
		public GenericSelector? ParentSelector { get; set; }
		[Parameter]
		public bool IsLast { get; set; }
		[Parameter]
		public EventCallback<string> AddedSelector { get; set; }
		[Parameter]
		public EventCallback<GenericSelector> RemovedSelector { get; set; }

		private bool IsCondition()
		{
			return Selector.Name == "or" || Selector.Name == "and" || Selector.Name == "not";
		}

		private void OnRemoveSelector(GenericSelector selector)
		{
			if (selector == null)
			{
				return;
			}

			Selector.Children.Remove(selector);
		}

		private void RemoveSelector()
		{
			RemovedSelector.InvokeAsync(Selector);
		}

		private async Task<IEnumerable<string>> SearchSelector(string value, CancellationToken token)
		{
			if (string.IsNullOrEmpty(value))
				return Storm.Selectors.Keys;
			return Storm.Selectors.Keys.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
		}

		private async Task<IEnumerable<string>> SearchAttributeValue(string value, CancellationToken token)
		{
			if (!SelectorParser.SelectorAttributes.TryGetValue(Selector.Name, out var attributeDict))
				return Enumerable.Empty<string>();

			if (!attributeDict.TryGetValue("name", out var values))
				return Enumerable.Empty<string>();

			if (string.IsNullOrWhiteSpace(value))
				return values;

			return values
				.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
		}

		private void OnAddSelector(string selector)
		{
			switch (selector)
			{
				case "and":
					Selector.Children.Add(new GenericSelector() { Name = "and", Children = new() { new GenericSelector() } });
					break;
				case "or":
					Selector.Children.Add(new GenericSelector() { Name = "or", Children = new() { new GenericSelector() } });
					break;
				case "not":
					Selector.Children.Add(new GenericSelector() { Name = "not", Children = new() { new GenericSelector() } });
					break;
				case "selector":
					Selector.Children.Add(new GenericSelector() { Name = "" });
					break;
				default:
					break;
			}
			StateHasChanged();
		}

		private void AddSelector(string selector)
		{
			AddedSelector.InvokeAsync(selector);
		}

		private void GetSelectorAttributes(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}

			var foundSelector = Storm.GetSelector(value);
			Selector.Name = foundSelector.Name;
			Selector.Attributes = foundSelector.Attributes;
			StateHasChanged();
		}
	}
}