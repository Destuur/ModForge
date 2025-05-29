namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class BuffUiVisibilityId : IAttribute
	{
		public BuffUiVisibilityId(string name, object value)
		{
			Name = name;
			Value = (BuffUiVisibility)Enum.Parse(typeof(BuffUiVisibility), value.ToString() ?? string.Empty);
		}

		public string Name { get; }
		public BuffUiVisibility Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = (BuffUiVisibility)Enum.Parse(typeof(BuffUiVisibility), value.ToString() ?? string.Empty); }
	}
}
