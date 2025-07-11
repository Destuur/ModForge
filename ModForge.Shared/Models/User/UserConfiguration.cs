﻿using ModForge.Shared.Models.Mods;

namespace ModForge.Shared.Models.User
{
	public class UserConfiguration
	{
		public string GameDirectory { get; set; } = string.Empty;
		public string NexusModsDirectory { get; set; } = string.Empty;
		public string Language { get; set; } = "en";
		public string UserName { get; set; } = string.Empty;
		public Dictionary<string, List<DropItem>> Loadouts { get; set; } = new();
	}
}
