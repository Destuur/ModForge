namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class Visibility : IAttribute
	{
		public Visibility(string name, object value)
		{
			Name = name;
			Value = (PerkVisibility)Enum.Parse(typeof(PerkVisibility), value.ToString() ?? "1");
		}

		public string Name { get; }
		public PerkVisibility Value { get; set; }
		object IAttribute.Value { get; set; }
	}
}
