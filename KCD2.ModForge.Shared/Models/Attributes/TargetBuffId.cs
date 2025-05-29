namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class TargetBuffId : IAttribute
	{
		public TargetBuffId(string name, object value)
		{
			Name = name;
			Value = value.ToString() ?? string.Empty;
		}

		public string Name { get; }
		public string Value { get; set; }
		object IAttribute.Value { get; set; }
	}
}
