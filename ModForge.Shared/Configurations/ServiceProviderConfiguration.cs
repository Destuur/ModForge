using Microsoft.Extensions.DependencyInjection;
using ModForge.Shared.Builders;
using ModForge.Shared.Builders.BuildHandlers;
using ModForge.Shared.Models.Abstractions;
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
				new MeleeWeaponBuildHandler<XElement, IModItem>(),
				new NPCToolBuildHandler<XElement, IModItem>(),
				new MiscItemBuildHandler<XElement, IModItem>(),
				new HoodBuildHandler<XElement, IModItem>(),
				new ArmorBuildHandler<XElement, IModItem>(),
				new MissileWeaponBuildHandler<XElement, IModItem>(),
				new DocumentBuildHandler<XElement, IModItem>(),
				new HerbBuildHandler<XElement, IModItem>(),
				new FoodBuildHandler<XElement, IModItem>(),
				new HelmetBuildHandler<XElement, IModItem>(),
				new DieBuildHandler<XElement, IModItem>(),
				new AmmoBuildHandler<XElement, IModItem>(),
				new ItemAliasBuildHandler<XElement, IModItem>(),
				new QuickSlotContainerBuildHandler<XElement, IModItem>(),
				new DiceBadgeBuildHandler<XElement, IModItem>(),
				new CraftingMaterialBuildHandler<XElement, IModItem>(),
				new PoisonBuildHandler<XElement, IModItem>(),
				new PickableItemBuildHandler<XElement, IModItem>(),
				new KeyBuildHandler<XElement, IModItem>(),
				new MoneyBuildHandler<XElement, IModItem>(),
				new KeyRingBuildHandler<XElement, IModItem>()
			};
			return service;
		}
	}

}
