using System.Xml.Serialization;

namespace SchemaParser.models;

[XmlRoot(ElementName="diagram")]
public class Diagram { 

    [XmlElement(ElementName="mxGraphModel")] 
    public MxGraphModel MxGraphModel { get; set; } 

    [XmlAttribute(AttributeName="name")] 
    public string Name { get; set; } 

    [XmlAttribute(AttributeName="id")] 
    public string Id { get; set; } 
}