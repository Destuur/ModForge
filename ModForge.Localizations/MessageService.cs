using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace ModForge.Localizations
{
	public sealed class MessageService
	{
		private readonly IStringLocalizer<MessageService> localizer;

		public MessageService(IStringLocalizer<MessageService> localizer)
		{
			this.localizer = localizer;
		}

		[return: NotNullIfNotNull(nameof(localizer))]
		public string? GetGreetingMessage()
		{
			return localizer["DashboardTitle"];
		}
	}
}
