using Microsoft.AspNetCore.Components;

namespace ModForge.Shared.Services
{
	public class NavigationService
	{
		private readonly NavigationManager navigationManager;
		private readonly Stack<string> backStack = new();
		private readonly Stack<string> forwardStack = new();
		private string? current;
		private bool isInternalNavigation = false;

		public NavigationService(NavigationManager navigationManager)
		{
			this.navigationManager = navigationManager;
			current = navigationManager.Uri;

			navigationManager.LocationChanged += (_, args) =>
			{
				if (isInternalNavigation) return;

				if (current != null)
				{
					backStack.Push(current);
				}

				forwardStack.Clear();
				current = args.Location;
			};
		}

		public bool CanGoBack => backStack.Count > 0;
		public bool CanGoForward => forwardStack.Count > 0;

		public void NavigateTo(string uri, bool forceLoad = false)
		{
			if (current != null)
			{
				backStack.Push(current);
			}
			forwardStack.Clear();

			isInternalNavigation = true;
			current = navigationManager.ToAbsoluteUri(uri).ToString();
			navigationManager.NavigateTo(uri, forceLoad);
			isInternalNavigation = false;
		}

		public void GoBack()
		{
			if (!CanGoBack) return;

			forwardStack.Push(current!);
			var target = backStack.Pop();

			isInternalNavigation = true;
			current = target;
			navigationManager.NavigateTo(target);
			isInternalNavigation = false;
		}

		public void GoForward()
		{
			if (!CanGoForward) return;

			backStack.Push(current!);
			var target = forwardStack.Pop();

			isInternalNavigation = true;
			current = target;
			navigationManager.NavigateTo(target);
			isInternalNavigation = false;
		}
	}
}
