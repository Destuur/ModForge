namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class Autolearnable : IAttribute
	{
		public Autolearnable(string name, object value)
		{
			Name = name;
			Value = bool.Parse(value.ToString() ?? "false");
		}

		public string Name { get; }
		public bool Value { get; set; }
		object IAttribute.Value => Value;
	}
}
