namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class Level : IAttribute
	{
		public Level(string name, object value)
		{
			Name = name;
			Value = int.Parse(value.ToString() ?? "1");
		}

		public string Name { get; }
		public int Value { get; }
		object IAttribute.Value => Value;
	}
}
