using KCD2.ModForge.Shared.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class Attribute<T> : IAttribute
	{
		public Attribute() { }

		public Attribute(string name, T value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; }
		public T Value { get; set; }
		object IAttribute.Value
		{
			get => Value;
			set => Value = (T)value;
		}

		private MathOperation ParseOperation(string op)
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

		private IList<BuffParam> ParseBuffParams(object value)
		{
			var list = new List<BuffParam>();
			var buffParams = value.ToString() ?? string.Empty;
			var pairs = buffParams.Split(',');

			foreach (var pair in pairs)
			{
				var match = System.Text.RegularExpressions.Regex.Match(pair.Trim(), @"(\w+)([\+\-\=\*\%\<\>\!])([\d\.]+)");
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
				}
				else
				{
					// Fehlerbehandlung
				}
			}

			return list;
		}
	}

	public class BuffParam
	{
		public BuffParam(string key, MathOperation operation, double value)
		{
			Key = key;
			Operation = operation;
			Value = value;
		}

		public string Key { get; set; }
		public MathOperation Operation { get; set; }
		public double Value { get; set; }
	}

	public enum MathOperation
	{
		AddAbs,      // +
		SubAbs,      // -
		SetAbs,      // =
		AddBaseRel,  // *
		AddCurrRel,  // %
		Min,         // <
		Max,         // >
		Negation     // !
	}
}
