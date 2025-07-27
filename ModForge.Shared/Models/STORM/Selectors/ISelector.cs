using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public interface ISelector 
	{ 

	}
	public interface IConditionalSelector
	{
		List<ISelector> Selectors { get; set; }
	}
}
