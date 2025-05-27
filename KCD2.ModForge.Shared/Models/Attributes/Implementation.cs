namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class Implementation : IAttribute
	{
		public Implementation(string name, object value)
		{
			Name = name;
			Value = value.ToString() ?? "Cpp:BasicTimed";
		}

		public string Name { get; }
		public string Value { get; protected set; }
		object IAttribute.Value => Value;
	}
}
