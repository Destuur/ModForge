namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class BuffLifetimeId : IAttribute
	{
		public BuffLifetimeId(string name, object value)
		{
			Name = name;
			Value = (BuffLifetime)Enum.Parse(typeof(BuffLifetime), value.ToString() ?? string.Empty);
		}

		public string Name { get; }
		public BuffLifetime Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = (BuffLifetime)Enum.Parse(typeof(BuffLifetime), value.ToString() ?? string.Empty); }
	}
}
