namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class Duration : IAttribute
	{
		public Duration(string name, object value)
		{
			Name = name;
			Value = double.Parse(value.ToString() ?? "0");
		}

		public string Name { get; }
		public double Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = double.Parse(value.ToString() ?? "0"); }
	}
}
