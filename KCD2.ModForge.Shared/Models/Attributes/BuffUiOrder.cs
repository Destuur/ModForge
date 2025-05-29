namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class BuffUiOrder : IAttribute
	{
		public BuffUiOrder(string name, object value)
		{
			Name = name;
			Value = int.Parse(value.ToString() ?? "1");
		}

		public string Name { get; }
		public int Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = int.Parse(value.ToString() ?? "1"); }
	}
}
