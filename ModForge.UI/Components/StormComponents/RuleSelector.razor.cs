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
		public bool IsLast { get; set; }
		[Parameter]
		public EventCallback<GenericSelector> AddedSelector { get; set; }

		private bool IsCondition()
		{
			return Selector.Name == "or" || Selector.Name == "and" || Selector.Name == "not";
		}

		private void OnAddSelector(GenericSelector selector)
		{
			if (selector is null)
			{
				return;
			}
			Selector.Children.Add(selector);
		}

		private void AddSelector(string selector)
		{
			switch (selector)
			{
				case "and":
					Selector.Children.Add(new GenericSelector() { Name = "and" });
					break;
				case "or":
					Selector.Children.Add(new GenericSelector() { Name = "or" });
					break;
				case "not":
					Selector.Children.Add(new GenericSelector() { Name = "not" });
					break;
				case "selector":
					Selector.Children.Add(new GenericSelector() { Name = "" });
					break;
				default:
					break;
			}
			StateHasChanged();
		}

		protected override void OnInitialized()
		{
			if (IsCondition())
			{
				Selector.Children.Add(new GenericSelector());
			}
		}
	}
}