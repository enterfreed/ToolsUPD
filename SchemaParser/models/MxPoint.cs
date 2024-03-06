using System.Xml.Serialization;

namespace SchemaParser.models;

[XmlRoot(ElementName="mxPoint")]
public class MxPoint { 

    [XmlAttribute(AttributeName="x")] 
    public decimal X { get; set; } 

    [XmlAttribute(AttributeName="y")] 
    public decimal Y { get; set; } 

    [XmlAttribute(AttributeName="as")] 
    public string As { get; set; } 
}