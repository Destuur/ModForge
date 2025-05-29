namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class IsPersistent : IAttribute
	{
		public IsPersistent(string name, object value)
		{
			Name = name;
			Value = bool.Parse(value.ToString() ?? "false");
		}

		public string Name { get; }
		public bool Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = bool.Parse(value.ToString() ?? "false"); }
	}
}
