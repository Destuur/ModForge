using Microsoft.Extensions.DependencyInjection;
using ModForge.Shared.Adapter;
using ModForge.Shared.Models.Data;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;

namespace ModForge.Shared
{
	public static class ServiceCollectionConfiguration
	{
		public static IServiceCollection AddModForgeServices(this IServiceCollection services)
		{
			services.AddSingleton<LocalizationService>();
			services.AddSingleton<ModService>();
			services.AddSingleton<UserConfigurationService>();
			services.AddSingleton<XmlService>();
			services.AddSingleton<ModCollection>();
			services.AddSingleton<DataSource>();
			return services;
		}

		public static IServiceCollection AddModForgeAdapters(this IServiceCollection services)
		{
			services.AddSingleton<LocalizationAdapter>();
			services.AddSingleton<JsonAdapter>();
			services.AddSingleton<IModItemAdapter, XmlAdapter>();
			return services;
		}
	}
}
