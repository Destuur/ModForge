namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class BuffExclusivityId : IAttribute
	{
		public BuffExclusivityId(string name, object value)
		{
			Name = name;
			Value = (BuffExclusivity)Enum.Parse(typeof(BuffExclusivity), value.ToString() ?? string.Empty);
		}

		public string Name { get; }
		public BuffExclusivity Value { get; protected set; }
		object IAttribute.Value => Value;
	}
}
