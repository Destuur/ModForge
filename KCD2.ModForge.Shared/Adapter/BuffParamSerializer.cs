using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.ModItems;
using System.Globalization;

namespace KCD2.ModForge.Shared.Adapter
{
	public partial class XmlAdapterOfT<T> where T : IModItem
	{
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
}
