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

		public string this[string key] => localizer[key];

		public string Get(string key, params object[] args) => localizer[key, args];
	}
}
