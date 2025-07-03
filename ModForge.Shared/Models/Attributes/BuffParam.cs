using ModForge.Shared.Models.Enums;
using System.Globalization;

namespace ModForge.Shared.Models.Attributes
{
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

		public BuffParam DeepClone()
		{
			return new BuffParam(Key, Operation, Value);
		}
	}

	public static class BuffParamSerializer
	{
		private static readonly Dictionary<MathOperation, string> OperatorMap = new()
		{
			{ MathOperation.AddAbsolute, "+" },
			{ MathOperation.SubtractAbsolute, "-" },
			{ MathOperation.SetAbsolute, "=" },
			{ MathOperation.AddRelativeToBase, "*" },
			{ MathOperation.MultiplyCurrent, "%" },
			{ MathOperation.Minimum, "<" },
			{ MathOperation.Maximum, ">" },
			{ MathOperation.NegateRelativeToValue, "!" }
		};

		public static string ToAttributeString(IEnumerable<BuffParam> parameters)
		{
			return string.Join(",", parameters.Select(p =>
			{
				if (!OperatorMap.TryGetValue(p.Operation, out var op))
					throw new InvalidOperationException($"Unsupported operation: {p.Operation}");

				return $"{p.Key}{op}{p.Value.ToString(CultureInfo.InvariantCulture)}";
			}));
		}
	}
}
