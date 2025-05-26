namespace KCD2.ModForge.Shared.Models.ModItems
{
	public class AttributeElement
	{
		public AttributeElement(string name, object value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; }
		public object Value { get; set; }
	}
}
