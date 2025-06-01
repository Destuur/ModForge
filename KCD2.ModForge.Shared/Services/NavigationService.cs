using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.Shared.Services
{
	public class NavigationService
	{
		private readonly NavigationManager _navManager;
		private readonly Stack<string> _backStack = new();
		private readonly Stack<string> _forwardStack = new();

		public Func<Task<bool>>? CanNavigateAsync { get; set; }

		public NavigationService(NavigationManager navManager)
		{
			_navManager = navManager;
		}

		public bool CanGoBack => _backStack.Count > 0;
		public bool CanGoForward => _forwardStack.Count > 0;

		public async Task NavigateToAsync(string uri)
		{
			if (!await CanProceedAsync()) return;

			var currentUri = _navManager.Uri;

			if (!string.Equals(currentUri, uri, StringComparison.OrdinalIgnoreCase))
			{
				_backStack.Push(currentUri);
				_forwardStack.Clear();
				_navManager.NavigateTo(uri);
			}
		}

		public async Task GoBackAsync()
		{
			if (!await CanProceedAsync()) return;

			if (_backStack.Count > 0)
			{
				var currentUri = _navManager.Uri;
				_forwardStack.Push(currentUri);

				var lastUri = _backStack.Pop();
				_navManager.NavigateTo(lastUri);
			}
		}

		public async Task GoForwardAsync()
		{
			if (!await CanProceedAsync()) return;

			if (_forwardStack.Count > 0)
			{
				var currentUri = _navManager.Uri;
				_backStack.Push(currentUri);

				var nextUri = _forwardStack.Pop();
				_navManager.NavigateTo(nextUri);
			}
		}

		private async Task<bool> CanProceedAsync()
		{
			if (CanNavigateAsync != null)
				return await CanNavigateAsync();
			return true;
		}
	}
}
