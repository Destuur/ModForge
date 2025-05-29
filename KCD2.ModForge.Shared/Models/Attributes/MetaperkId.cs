namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class MetaperkId : IAttribute
	{
		public MetaperkId(string name, object value)
		{
			Name = name;
			Value = value.ToString() ?? string.Empty;
		}

		public string Name { get; }
		public string Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = value.ToString() ?? string.Empty; }
	}
}
