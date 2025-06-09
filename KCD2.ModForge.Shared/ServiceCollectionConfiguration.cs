using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models.Data;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KCD2.ModForge.Shared
{
	public static class ServiceCollectionConfiguration
	{
		public static IServiceCollection AddModForgeServices(this IServiceCollection services)
		{
			services.AddSingleton<LocalizationService>();
			services.AddSingleton<ModService>();
			services.AddScoped<NavigationService>();
			services.AddSingleton<UserConfigurationService>();
			services.AddSingleton<XmlToJsonService>();
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
