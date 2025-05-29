namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class BuffHcModeUiVisibilityId : IAttribute
	{
		public BuffHcModeUiVisibilityId(string name, object value)
		{
			Name = name;
			Value = int.Parse(value.ToString() ?? "0");
		}

		public string Name { get; }
		public int Value { get; set; }
		object IAttribute.Value { get => Value; set => int.Parse(value.ToString() ?? "0"); }
	}
}
