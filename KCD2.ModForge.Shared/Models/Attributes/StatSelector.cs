namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class StatSelector : IAttribute
	{
		public StatSelector(string name, object value)
		{
			Name = name;
			Value = (StatType)Enum.Parse(typeof(StatType), value.ToString() ?? "0");
		}

		public string Name { get; }
		public StatType Value { get; set; }
		object IAttribute.Value { get; set; }
	}
}
