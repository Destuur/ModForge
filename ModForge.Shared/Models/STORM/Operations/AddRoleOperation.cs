namespace ModForge.Shared.Models.STORM.Operations
{
	public class AddRoleOperation : IOperation, ICustomOperation
	{
		public string Name { get; set; }
	}
	public class SetUiNameOperation : IOperation, ICustomOperation
	{
		public string Name { get; set; }
	}
	public class SetReputationOperation : IOperation, ICustomOperation
	{
		public double? Value { get; set; }
	}
	public class SetInventoryOperation : IOperation, ICustomOperation
	{
		public string Name { get; set; }
	}
	public class AddInventoryOperation : IOperation, ICustomOperation
	{
		public string Preset { get; set; }
	}
	public class SetBodyOperation : IOperation, ICustomOperation
	{
		public string Name { get; set; }
	}
	public class SetHeadOperation : IOperation, ICustomOperation
	{
		public string Name { get; set; }
	}
	public class AddContextOperation : IOperation, ICustomOperation
	{
		public string Name { get; set; }
	}
	public class AddMetaroleOperation : IOperation, ICustomOperation
	{
		public string Name { get; set; }
	}
}
