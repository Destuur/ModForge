using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.Shared.Services
{
	public class NavigationService
	{
		private readonly NavigationManager _navManager;
		private readonly Stack<string> _backStack = new();
		private readonly Stack<string> _forwardStack = new();

		public NavigationService(NavigationManager navManager)
		{
			_navManager = navManager;
		}

		public void NavigateTo(string uri)
		{
			var currentUri = _navManager.Uri;

			if (!string.Equals(currentUri, uri, StringComparison.OrdinalIgnoreCase))
			{
				_backStack.Push(currentUri);
				_forwardStack.Clear(); // Neue Navigation löscht den Forward-Stack
				_navManager.NavigateTo(uri);
			}
		}

		public void GoBack()
		{
			if (_backStack.Count > 0)
			{
				var currentUri = _navManager.Uri;
				_forwardStack.Push(currentUri);

				var lastUri = _backStack.Pop();
				_navManager.NavigateTo(lastUri);
			}
		}

		public void GoForward()
		{
			if (_forwardStack.Count > 0)
			{
				var currentUri = _navManager.Uri;
				_backStack.Push(currentUri);

				var nextUri = _forwardStack.Pop();
				_navManager.NavigateTo(nextUri);
			}
		}

		public bool CanGoBack => _backStack.Count > 0;
		public bool CanGoForward => _forwardStack.Count > 0;
	}
}
