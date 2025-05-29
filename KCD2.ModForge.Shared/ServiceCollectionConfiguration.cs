using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Factories;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Mods;
using KCD2.ModForge.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.Shared
{
	public static class ServiceCollectionConfiguration
	{
		public static IServiceCollection AddModForgeServices(this IServiceCollection services)
		{

			services.AddSingleton<IconService>();
			services.AddSingleton<LocalizationService>();
			services.AddSingleton<ModService>();
			services.AddSingleton<OrchestrationService>();
			services.AddSingleton<PerkService>();
			services.AddSingleton<UserConfigurationService>();
			services.AddSingleton<XmlToJsonService>();
			services.AddSingleton<ModCollection>();
			return services;
		}

		public static IServiceCollection AddModForgeAdapters(this IServiceCollection services)
		{
			services.AddSingleton<LocalizationAdapter>();
			services.AddSingleton<JsonAdapterOfT<Perk>>();
			services.AddSingleton<JsonAdapterOfT<Buff>>();
			services.AddSingleton<XmlAdapterOfT<Perk>>();
			services.AddSingleton<XmlAdapterOfT<Buff>>();
			return services;
		}
	}
}
