using System.Xml.Serialization;

namespace KCD2.XML.Tool.Shared.Models
{
	public interface IModItem
	{
		public string Id { get; }
		public string Path { get; }
	}
}
