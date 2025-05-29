namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class BuffClassId : IAttribute
	{
		public BuffClassId(string name, object value)
		{
			Name = name;
			Value = (BuffClass)Enum.Parse(typeof(BuffClass), value.ToString() ?? "4");
		}

		public string Name { get; }
		public BuffClass Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = (BuffClass)Enum.Parse(typeof(BuffClass), value.ToString() ?? "4"); }
	}
}
