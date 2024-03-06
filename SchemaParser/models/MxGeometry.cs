using System.Xml.Serialization;

namespace SchemaParser.models;

[XmlRoot(ElementName="mxGeometry")]
public class MxGeometry
{
    [XmlAttribute(AttributeName="x")] 
    public decimal X { get; set; } 

    [XmlAttribute(AttributeName="y")] 
    public decimal Y { get; set; } 

    [XmlAttribute(AttributeName="width")] 
    public double Width { get; set; } 

    [XmlAttribute(AttributeName="height")] 
    public int Height { get; set; } 

    [XmlAttribute(AttributeName="as")] 
    public string As { get; set; } 

    [XmlAttribute(AttributeName="xz")] 
    public int Xz { get; set; } 

    [XmlAttribute(AttributeName="relative")] 
    public int Relative { get; set; } 

    [XmlElement(ElementName="Array")] 
    public Array Array { get; set; } 

    [XmlElement(ElementName="mxPoint")] 
    public MxPoint MxPoint { get; set; } 
}