using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Factories;
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
			services.AddSingleton(new List<IDataPoint>() {
				new DataPoint(ToolResources.Keys.TablesPath(), "perk__combat", typeof(Perk)),
				new DataPoint(ToolResources.Keys.TablesPath(), "perk__hardcore", typeof(Perk)),
				new DataPoint(ToolResources.Keys.TablesPath(), "perk__kcd2", typeof(Perk)),
				new DataPoint(ToolResources.Keys.TablesPath(), "buff.xml", typeof(Buff)),
				new DataPoint(ToolResources.Keys.TablesPath(), "buff__alchemy", typeof(Buff)),
				new DataPoint(ToolResources.Keys.TablesPath(), "buff__perk", typeof(Buff)),
				new DataPoint(ToolResources.Keys.TablesPath(), "buff__perk_hardcore", typeof(Buff)),
				new DataPoint(ToolResources.Keys.TablesPath(), "buff__perk_kcd1", typeof(Buff))
			});
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
