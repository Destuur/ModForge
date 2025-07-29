using Microsoft.AspNetCore.Components;
using ModForge.Shared.Models.STORM.Operations;

namespace ModForge.UI.Components.StormComponents
{
	public partial class OperationComponent
	{
		[Parameter]
		public GenericOperation? Operation { get; set; }
	}
}