namespace KCD2.ModForge.Shared.Models.Attributes
{
	public class ExcludeInGameMode : IAttribute
	{
		public ExcludeInGameMode(string name, object value)
		{
			Name = name;
			Value = (GameMode)Enum.Parse(typeof(GameMode), value.ToString() ?? "1");
		}

		public string Name { get; }
		public GameMode Value { get; set; }
		object IAttribute.Value { get => Value; set => Value = (GameMode)Enum.Parse(typeof(GameMode), value.ToString() ?? "1"); }
	}
}
