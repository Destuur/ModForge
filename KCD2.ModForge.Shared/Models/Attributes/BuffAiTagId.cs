namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class BuffAiTagId : IAttribute
	{
		public BuffAiTagId(string name, object value)
		{
			Name = name;
			Value = (BuffAiTag)Enum.Parse(typeof(BuffAiTag), value.ToString() ?? "0");
		}

		public string Name { get; }
		public BuffAiTag Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = (BuffAiTag)Enum.Parse(typeof(BuffAiTag), value.ToString() ?? "0"); }
	}
}
