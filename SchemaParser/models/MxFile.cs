using System.Xml.Serialization;

namespace SchemaParser.models;

[XmlRoot(ElementName="mxfile")]
public class MxFile { 

    [XmlElement(ElementName="diagram")] 
    public Diagram Diagram { get; set; } 

    [XmlAttribute(AttributeName="host")] 
    public string Host { get; set; } 

    [XmlAttribute(AttributeName="modified")] 
    public DateTime Modified { get; set; } 

    [XmlAttribute(AttributeName="agent")] 
    public string Agent { get; set; } 

    [XmlAttribute(AttributeName="etag")] 
    public string Etag { get; set; } 

    [XmlAttribute(AttributeName="version")] 
    public string Version { get; set; } 

    [XmlAttribute(AttributeName="type")] 
    public string Type { get; set; } 
}