using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.STORM;
using ModForge.Shared.Models.STORM.Selectors;
using ModForge.Shared.Services;

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
	}
}