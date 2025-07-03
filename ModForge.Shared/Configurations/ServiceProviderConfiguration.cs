using Microsoft.Extensions.DependencyInjection;
using ModForge.Shared.Builders;
using ModForge.Shared.Builders.BuildHandlers;
using ModForge.Shared.Models.ModItems;
using System.Xml.Linq;

namespace ModForge.Shared.Configurations
{
	public static class ServiceProviderConfiguration
	{
		public static IServiceProvider AddBuildHandler(this IServiceProvider service)
		{
			var builder = service.GetRequiredService<IBuilder<XElement, IModItem>>();
			builder.Handlers = new List<IBuildHandler<XElement, IModItem>>()
			{
				new PerkBuildHandler<XElement, IModItem>(),
				new BuffBuildHandler<XElement, IModItem>(),
				new MeleeWeaponBuildHandler<XElement, IModItem>()
			};
			return service;
		}
	}

}
