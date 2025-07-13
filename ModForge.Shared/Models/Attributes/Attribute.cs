using ModForge.Shared.Factories;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.ModItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModForge.Shared.Models.Attributes
{
	public class Attribute<T> : IAttribute
	{
		public Attribute() { }

		public Attribute(string name, T value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; } = string.Empty;
		public T Value { get; set; } = default!;
		object IAttribute.Value
		{
			get => Value!;
			set => Value = (T)value;
		}

		public IAttribute DeepClone()
		{
			if (Value is IList<BuffParam> buffParams)
			{
				return new Attribute<IList<BuffParam>>(Name, buffParams.Select(attr => attr.DeepClone()).ToList());
			}
			if (Value is WeaponClasss weaponClass)
			{
				return new Attribute<WeaponClasss>(Name, weaponClass);
			}
			return new Attribute<T>(Name, Value);
		}

		//private MathOperation ParseOperation(string op)
		//{
		//	return op switch
		//	{
		//		"+" => MathOperation.AddAbsolute,
		//		"-" => MathOperation.SubtractAbsolute,
		//		"=" => MathOperation.SetAbsolute,
		//		"*" => MathOperation.AddRelativeToBase,
		//		"%" => MathOperation.MultiplyCurrent,
		//		"<" => MathOperation.Minimum,
		//		">" => MathOperation.Maximum,
		//		"!" => MathOperation.NegateRelativeToValue,
		//		_ => throw new InvalidOperationException($"Unbekannte Operation: {op}")
		//	};
		//}

		//private IList<BuffParam> ParseBuffParams(object value)
		//{
		//	var list = new List<BuffParam>();
		//	var buffParams = value.ToString() ?? string.Empty;
		//	var pairs = buffParams.Split(',');

		//	foreach (var pair in pairs)
		//	{
		//		var match = System.Text.RegularExpressions.Regex.Match(pair.Trim(), @"(\w+)([\+\-\=\*\%\<\>\!])([\d\.]+)");
		//		if (match.Success)
		//		{
		//			string key = match.Groups[1].Value;
		//			string opStr = match.Groups[2].Value;
		//			string valStr = match.Groups[3].Value;

		//			if (double.TryParse(valStr, System.Globalization.NumberStyles.Any,
		//								System.Globalization.CultureInfo.InvariantCulture,
		//								out double val))
		//			{
		//				MathOperation op = ParseOperation(opStr);
		//				list.Add(new BuffParam(key, op, val));
		//			}
		//		}
		//		else
		//		{
		//			// Fehlerbehandlung
		//		}
		//	}

		//	return list;
		//}
	}
}
