namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class SkillSelector : IAttribute
	{
		public SkillSelector(string name, object value)
		{
			Name = name;
			Value = (SkillType)Enum.Parse(typeof(SkillType), value.ToString() ?? "4");
		}

		public string Name { get; }
		public SkillType Value { get; protected set; }
		object IAttribute.Value => Value;
	}
}
