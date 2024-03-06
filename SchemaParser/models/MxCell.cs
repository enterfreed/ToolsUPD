using System.Xml.Serialization;
using SchemaParser.models;

namespace SchemaParser;

[XmlRoot(ElementName="mxCell")]
public class MxCell 
{ 
	[XmlAttribute(AttributeName="id")] 
	public string Id { get; set; } 

	[XmlAttribute(AttributeName="parent")] 
	public string Parent { get; set; } 

	[XmlElement(ElementName="mxGeometry")] 
	public MxGeometry? MxGeometry { get; set; } 

	[XmlAttribute(AttributeName="style")] 
	public string? Style { get; set; }
	
	[XmlAttribute(AttributeName="vertex")] 
	public int Vertex { get; set; } 

	[XmlAttribute(AttributeName="value")] 
	public string? Value { get; set; } 

	[XmlAttribute(AttributeName="source")] 
	public string? Source { get; set; } 

	[XmlAttribute(AttributeName="target")] 
	public string? Target { get; set; }
	
	[XmlAttribute(AttributeName="edge")] 
	public int Edge { get; set; } 

	public override string ToString()
	{
		return Id;
	}
}

class Helper : IEqualityComparer<MxCell>
{
	public bool Equals(MxCell x, MxCell y)
	{
		if (ReferenceEquals(x, y)) return true;
		if (ReferenceEquals(x, null)) return false;
		if (ReferenceEquals(y, null)) return false;
		if (x.GetType() != y.GetType()) return false;
		return x.Id == y.Id;
	}

	public int GetHashCode(MxCell obj)
	{
		return obj.Id.GetHashCode();
	}
}