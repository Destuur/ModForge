namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class BuffFamilyId : IAttribute
	{
		public BuffFamilyId(string name, object value)
		{
			Name = name;
			Value = (BuffFamily)Enum.Parse(typeof(BuffFamily), value.ToString() ?? string.Empty);
		}

		public string Name { get; }
		public BuffFamily Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = (BuffFamily)Enum.Parse(typeof(BuffFamily), value.ToString() ?? string.Empty); }
	}
}
