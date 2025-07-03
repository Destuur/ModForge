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
				return element.Name.LocalName.ToLower() == typeof(MeleeWeapon).Name.ToLower();
			}
			return false;
		}
	}

	public class NPCToolBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new NPCTool()
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
				return element.Name.LocalName.ToLower() == typeof(NPCTool).Name.ToLower();
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
				return element.Name.LocalName.ToLower() == typeof(Perk).Name.ToLower();
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
				return element.Name.LocalName.ToLower() == typeof(Buff).Name.ToLower();
			}
			return false;
		}
	}

	public class MiscItemBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new MiscItem()
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
				return element.Name.LocalName.ToLower() == typeof(MiscItem).Name.ToLower();
			}
			return false;
		}
	}

	public class HoodBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Hood()
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
				return element.Name.LocalName.ToLower() == typeof(Hood).Name.ToLower();
			}
			return false;
		}
	}

	public class ArmorBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Armor()
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
				return element.Name.LocalName.ToLower() == typeof(Armor).Name.ToLower();
			}
			return false;
		}
	}

	public class MissileWeaponBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new MissileWeapon()
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
				return element.Name.LocalName.ToLower() == typeof(MissileWeapon).Name.ToLower();
			}
			return false;
		}
	}

	public class DocumentBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Document()
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
				return element.Name.LocalName.ToLower() == typeof(Document).Name.ToLower();
			}
			return false;
		}
	}

	public class HerbBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Herb()
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
				return element.Name.LocalName.ToLower() == typeof(Herb).Name.ToLower();
			}
			return false;
		}
	}

	public class FoodBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Food()
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
				return element.Name.LocalName.ToLower() == typeof(Food).Name.ToLower();
			}
			return false;
		}
	}

	public class HelmetBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Helmet()
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
				return element.Name.LocalName.ToLower() == typeof(Helmet).Name.ToLower();
			}
			return false;
		}
	}

	public class DieBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Die()
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
				return element.Name.LocalName.ToLower() == typeof(Die).Name.ToLower();
			}
			return false;
		}
	}

	public class AmmoBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Ammo()
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
				return element.Name.LocalName.ToLower() == typeof(Ammo).Name.ToLower();
			}
			return false;
		}
	}

	public class ItemAliasBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new ItemAlias()
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
				return element.Name.LocalName.ToLower() == typeof(ItemAlias).Name.ToLower();
			}
			return false;
		}
	}

	public class QuickSlotContainerBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new QuickSlotContainer()
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
				return element.Name.LocalName.ToLower() == typeof(QuickSlotContainer).Name.ToLower();
			}
			return false;
		}
	}

	public class DiceBadgeBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new DiceBadge()
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
				return element.Name.LocalName.ToLower() == typeof(DiceBadge).Name.ToLower();
			}
			return false;
		}
	}

	public class CraftingMaterialBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new CraftingMaterial()
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
				return element.Name.LocalName.ToLower() == typeof(CraftingMaterial).Name.ToLower();
			}
			return false;
		}
	}

	public class PoisonBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Poison()
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
				return element.Name.LocalName.ToLower() == typeof(Poison).Name.ToLower();
			}
			return false;
		}
	}

	public class PickableItemBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new PickableItem()
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
				return element.Name.LocalName.ToLower() == typeof(PickableItem).Name.ToLower();
			}
			return false;
		}
	}

	public class KeyBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Key()
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
				return element.Name.LocalName.ToLower() == typeof(Key).Name.ToLower();
			}
			return false;
		}
	}

	public class MoneyBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new Money()
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
				return element.Name.LocalName.ToLower() == typeof(Money).Name.ToLower();
			}
			return false;
		}
	}

	public class KeyRingBuildHandler<TInput, TOutput> : IBuildHandler<TInput, TOutput> where TOutput : class
	{
		public TOutput? Handle(TInput input)
		{
			if (input is XElement element)
			{
				if (element is null)
				{
					return null;
				}

				return new KeyRing()
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
				return element.Name.LocalName.ToLower() == typeof(KeyRing).Name.ToLower();
			}
			return false;
		}
	}
}
