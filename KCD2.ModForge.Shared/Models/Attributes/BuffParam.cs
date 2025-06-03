namespace KCD2.ModForge.Shared.Models.Attributes
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
}
