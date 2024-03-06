using System.Xml.Serialization;

namespace SchemaParser.models;

[XmlRoot(ElementName="Array")]
public class Array { 

    [XmlElement(ElementName="mxPoint")] 
    public List<MxPoint> MxPoint { get; set; } 

    [XmlAttribute(AttributeName="as")] 
    public string As { get; set; } 
}