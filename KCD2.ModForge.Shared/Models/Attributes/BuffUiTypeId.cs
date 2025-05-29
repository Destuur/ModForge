namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class BuffUiTypeId : IAttribute
	{
		public BuffUiTypeId(string name, object value)
		{
			Name = name;
			Value = (BuffUiType)Enum.Parse(typeof(BuffUiType), value.ToString() ?? "0");
		}

		public string Name { get; }
		public BuffUiType Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = (BuffUiType)Enum.Parse(typeof(BuffUiType), value.ToString() ?? "0"); }
	}
}
