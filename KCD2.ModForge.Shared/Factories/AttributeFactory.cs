using KCD2.ModForge.Shared.Models.Attributes;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace KCD2.ModForge.Shared.Factories
{
	public static class AttributeFactory
	{
		// TODO: Vielleicht ein Dictionary<string<string, Type> machen in dem auch der Anzeigename in der App stehen könnte?
		private static readonly Dictionary<string, Type> AttributeTypeMap = new()
		{
			{ "autolearnable", typeof(bool) },
			{ "buff_ai_tag_id", typeof(BuffAiTag) },
			{ "buff_class_id", typeof(BuffClass) },
			{ "buff_desc", typeof(string) },
			{ "buff_exclusivity_id", typeof(BuffExclusivity) },
			{ "buff_family_id", typeof(BuffFamily) },
			{ "buff_hc_mode_ui_visibility_id", typeof(int) },
			{ "buff_id", typeof(string) },
			{ "buff_lifetime_id", typeof(BuffLifetime) },
			{ "buff_name", typeof(string) },
			{ "buff_params", typeof(IList<BuffParam>) },
			{ "buff_ui_name", typeof(string) },
			{ "buff_ui_order", typeof(int) },
			{ "buff_ui_type_id", typeof(BuffUiType) },
			{ "buff_ui_visibility_id", typeof(BuffUiVisibility) },
			{ "combat_technique_id", typeof(string) },
			{ "duration", typeof(double) },
			{ "exclude_in_game_mode", typeof(ExcludeInGameMode) },
			{ "first_perk_id", typeof(string) },
			{ "icon_id", typeof(string) },
			{ "implementation", typeof(string) },
			{ "is_persistent", typeof(bool) },
			{ "level", typeof(int) },
			{ "metaperk_id", typeof(string) },
			{ "parent_id", typeof(string) },
			{ "perk_id", typeof(string) },
			{ "perk_name", typeof(string) },
			{ "perk_ui_desc", typeof(string) },
			{ "perk_ui_lore_desc", typeof(string) },
			{ "perk_ui_name", typeof(string) },
			{ "second_perk_id", typeof(string) },
			{ "skill_selector", typeof(SkillSelector) },
			{ "slot_buff_ui_name", typeof(string) },
			// Vielleicht die Ziffern am Ende des Icon Namens?
			{ "slot_icon_id", typeof(int) },
			{ "source_buff_id", typeof(string) },
			{ "stat_selector", typeof(StatSelector) },
			{ "target_buff_id", typeof(string) },
			{ "ui_priority", typeof(int) },
			{ "visibility", typeof(Visibility) },
			{ "visual_effect", typeof(string) },
		};

		public static IAttribute CreateAttribute(string name, string valueStr)
		{
			if (!AttributeTypeMap.TryGetValue(name, out var type))
				throw new InvalidOperationException($"No type mapping defined for attribute '{name}'.");

			object value;

			try
			{
				if (type == typeof(IList<BuffParam>))
					value = (IList<BuffParam>)ParseBuffParams(valueStr);
				else if (type.IsEnum)
					value = Enum.Parse(type, valueStr);
				else
					value = Convert.ChangeType(valueStr, type, CultureInfo.InvariantCulture);
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
					throw new InvalidOperationException($"Konstruktor für {genericType.Name} nicht gefunden.");

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
				"+" => MathOperation.AddAbs,
				"-" => MathOperation.SubAbs,
				"=" => MathOperation.SetAbs,
				"*" => MathOperation.AddBaseRel,
				"%" => MathOperation.AddCurrRel,
				"<" => MathOperation.Min,
				">" => MathOperation.Max,
				"!" => MathOperation.Negation,
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

					// Versuch, mit Regex zu matchen: key + operation + value
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
							// Fehlerbehandlung falls Wert nicht geparst werden kann
							throw new InvalidOperationException($"Ungültiger Wert '{valStr}' für Key '{key}'.");
						}
					}
					else
					{
						// Kein Operation+Wert gefunden → Key ist ein Flag (z.B. LimitSprint)
						// Hier interpretieren wir es als Set-Operation mit Wert 1 (aktiv)
						list.Add(new BuffParam(trimmed, MathOperation.SetAbs, 1));
					}
				}
				return list;
			}
			catch (Exception e)
			{

				throw new Exception();
			}

		}

	}
}
