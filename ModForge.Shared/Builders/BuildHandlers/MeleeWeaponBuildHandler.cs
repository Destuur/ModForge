using ModForge.Shared.Factories;
using ModForge.Shared.Models.ModItems;
using System.Xml.Linq;

namespace ModForge.Shared.Builders.BuildHandlers
{
	public class MeleeWeaponBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new MeleeWeapon()
				{
					Id = (string)element.Attribute("Id")!,
					Path = string.Join("/", element.AncestorsAndSelf().Reverse().Select(a => a.Name.LocalName).ToArray()),
					Attributes = element.Attributes().Select(attr => AttributeFactory.CreateAttribute(attr.Name.LocalName, attr.Value)).ToList()
				} as TOutput;
			}
			return null;
		}

		public bool IsResponsible(TInput input)
		{
			if (input is XElement element)
			{
				return element.Name.LocalName == typeof(MeleeWeapon).ToString();
			}
			return false;
		}
	}

	public class PerkBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Perk()
				{
					Id = (string)element.Attribute("Id")!,
					Path = string.Join("/", element.AncestorsAndSelf().Reverse().Select(a => a.Name.LocalName).ToArray()),
					Attributes = element.Attributes().Select(attr => AttributeFactory.CreateAttribute(attr.Name.LocalName, attr.Value)).ToList()
				} as TOutput;
			}
			return null;
		}

		public bool IsResponsible(TInput input)
		{
			if (input is XElement element)
			{
				return element.Name.LocalName == typeof(Perk).ToString();
			}
			return false;
		}
	}

	public class BuffBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Buff()
				{
					Id = (string)element.Attribute("Id")!,
					Path = string.Join("/", element.AncestorsAndSelf().Reverse().Select(a => a.Name.LocalName).ToArray()),
					Attributes = element.Attributes().Select(attr => AttributeFactory.CreateAttribute(attr.Name.LocalName, attr.Value)).ToList()
				} as TOutput;
			}
			return null;
		}

		public bool IsResponsible(TInput input)
		{
			if (input is XElement element)
			{
				return element.Name.LocalName == typeof(Buff).ToString();
			}
			return false;
		}
	}
}
