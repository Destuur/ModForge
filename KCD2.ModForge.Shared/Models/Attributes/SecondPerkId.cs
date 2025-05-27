namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class SecondPerkId : IAttribute
	{
		public SecondPerkId(string name, object value)
		{
			Name = name;
			Value = value.ToString() ?? string.Empty;
		}

		public string Name { get; }
		public string Value { get; protected set; }
		object IAttribute.Value => Value;
	}
}
