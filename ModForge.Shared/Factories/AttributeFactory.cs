using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Enums;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ModForge.Shared.Factories
{
	public static class AttributeFactory
	{
		private static readonly Dictionary<string, Type> knownAttributeEnums = new()
		{
			{ "buff_ai_tag_id", typeof(BuffAiTag) },
			{ "buff_class_id", typeof(BuffClass) },
			{ "buff_exclusivity_id", typeof(BuffExclusivity) },
			{ "buff_family_id", typeof(BuffFamily) },
			{ "buff_lifetime_id", typeof(BuffLifetime) },
			{ "buff_ui_type_id", typeof(BuffUiType) },
			{ "buff_ui_visibility_id", typeof(BuffUiVisibility) },
			{ "exclude_in_game_mode", typeof(ExcludeInGameMode) },
			{ "stat_selector", typeof(StatSelector) },
			{ "visibility", typeof(Visibility) },
			{ "AmmoClass", typeof(AmmoClass) },
			{ "ArmorArchetype", typeof(ArmorArchetype) },
			{ "ArmorSurface", typeof(ArmorSurface) },
			{ "BodyLayerType", typeof(BodyLayerType) },
			{ "CraftingMaterialSubtype", typeof(CraftingMaterialSubtype) },
			{ "CraftingMaterialType", typeof(CraftingMaterialType) },
			{ "DiceBadgeSubtype", typeof(DiceBadgeSubtype) },
			{ "DiceBadgeType", typeof(DiceBadgeType) },
			{ "DocumentClass", typeof(DocumentClass) },
			{ "FoodSubtype", typeof(FoodSubtype) },
			{ "FoodType", typeof(FoodType) },
			{ "ItemCategory", typeof(ItemCategory) },
			{ "ItemTag", typeof(ItemTag) },
			{ "ItemUiSound", typeof(ItemUiSound) },
			{ "KeySubtype", typeof(KeySubtype) },
			{ "KeyType", typeof(KeyType) },
			{ "MiscSubtype", typeof(MiscSubtype) },
			{ "MiscType", typeof(MiscType) },
			{ "NpcToolSubtype", typeof(NpcToolSubtype) },
			{ "OintmentItemSubtype", typeof(OintmentItemSubtype) },
			{ "OintmentItemType", typeof(OintmentItemType) },
			{ "WeaponSubClass", typeof(WeaponSubClass) }
		};

		// TODO: Vielleicht ein Dictionary<string<string, Type> machen in dem auch der Anzeigename in der App stehen könnte?
		private static readonly Dictionary<string, Type> attributeTypeMap = new();

		public static IAttribute CreateAttribute(string name, string valueStr)
		{
			if (!attributeTypeMap.TryGetValue(name, out var type))
			{
				throw new InvalidOperationException($"No type mapping defined for attribute '{name}'.");
			}

			object value;

			try
			{
				if (type == typeof(IList<BuffParam>))
				{
					value = (IList<BuffParam>)ParseBuffParams(valueStr);
				}
				else if (type.IsEnum && valueStr == "")
				{
					value = Enum.Parse(type, "0");
				}
				else if (type.IsEnum)
				{
					value = Enum.Parse(type, valueStr);
				}
				else if (type == typeof(int) && valueStr == "" || type == typeof(double) && valueStr == "")
				{
					value = 0;
				}
				else if (type == typeof(bool) && valueStr == "")
				{
					value = false;
				}
				else
				{
					value = Convert.ChangeType(valueStr, type, CultureInfo.InvariantCulture);
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Wert konnte nicht in Typ '{type.Name}' umgewandelt werden: '{valueStr}'", ex);
			}

			Type genericType = typeof(Attribute<>).MakeGenericType(type);

			try
			{
				var constructor = genericType.GetConstructor(new[] { typeof(string), type });
				if (constructor == null)
				{
					throw new InvalidOperationException($"Konstruktor für {genericType.Name} nicht gefunden.");
				}

				var attribute = (IAttribute)constructor.Invoke(new[] { name, value });
				return attribute;
			}
			catch (Exception)
			{

				throw new Exception();
			}
		}

		private static MathOperation ParseOperation(string op)
		{
			return op switch
			{
				"+" => MathOperation.AddAbsolute,
				"-" => MathOperation.SubtractAbsolute,
				"=" => MathOperation.SetAbsolute,
				"*" => MathOperation.AddRelativeToBase,
				"%" => MathOperation.MultiplyCurrent,
				"<" => MathOperation.Minimum,
				">" => MathOperation.Maximum,
				"!" => MathOperation.NegateRelativeToValue,
				_ => throw new InvalidOperationException($"Unbekannte Operation: {op}")
			};
		}

		private static IList<BuffParam> ParseBuffParams(object value)
		{
			try
			{
				var list = new List<BuffParam>();
				var buffParams = value.ToString() ?? string.Empty;
				var pairs = buffParams.Split(',');

				foreach (var pair in pairs)
				{
					var trimmed = pair.Trim();

					var match = System.Text.RegularExpressions.Regex.Match(trimmed, @"(\w+)([\+\-\=\*\%\<\>\!])([\-\+]?\d+(\.\d+)?)");

					if (match.Success)
					{
						string key = match.Groups[1].Value;
						string opStr = match.Groups[2].Value;
						string valStr = match.Groups[3].Value;

						if (double.TryParse(valStr, System.Globalization.NumberStyles.Any,
											System.Globalization.CultureInfo.InvariantCulture,
											out double val))
						{
							MathOperation op = ParseOperation(opStr);
							list.Add(new BuffParam(key, op, val));
						}
						else
						{
							throw new InvalidOperationException($"Ungültiger Wert '{valStr}' für Key '{key}'.");
						}
					}
					else
					{
						list.Add(new BuffParam(trimmed, MathOperation.SetAbsolute, 1));
					}
				}
				return list;
			}
			catch (Exception e)
			{

				throw new Exception();
			}

		}

		public static void DiscoverAndAddAttributeTypes(XDocument xmlDoc)
		{
			if (xmlDoc.Root == null)
				return;

			TraverseElements(xmlDoc.Root);
		}

		private static void TraverseElements(XElement element)
		{
			// Analysiere alle Attribute des aktuellen Elements
			foreach (var attr in element.Attributes())
			{
				var name = attr.Name.LocalName;
				var value = attr.Value;

				if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
					continue;

				if (!attributeTypeMap.ContainsKey(name))
				{
					Type inferredType = InferType(name, value);
					attributeTypeMap[name] = inferredType;
					Console.WriteLine($"[DISCOVERED] \"{name}\" → {inferredType.Name}");
				}
			}

			// Rekursiv alle Kindelemente analysieren
			foreach (var child in element.Elements())
			{
				TraverseElements(child);
			}
		}

		private static Type InferType(string name, string value)
		{
			if (knownAttributeEnums.TryGetValue(name, out var enumType))
				return enumType;

			if (name == "buff_params")
				return typeof(IList<BuffParam>);

			if (name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
				name.EndsWith("Id", StringComparison.OrdinalIgnoreCase) ||
				Guid.TryParse(value, out _))
			{
				return typeof(string);
			}

			if (bool.TryParse(value, out _))
				return typeof(bool);

			if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
				return typeof(float);

			if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
				return typeof(int);

			return typeof(string);
		}

		public static IList<IAttribute> GetAllAttributes()
		{
			var newList = new List<IAttribute>();

			foreach (var attribute in attributeTypeMap)
			{
				newList.Add(CreateAttribute(attribute.Key, ""));
			}

			return newList;
		}

	}
}
