using System.Xml.Serialization;

namespace SchemaParser.models;

[XmlRoot(ElementName="root")]
public class Root { 

    [XmlElement(ElementName="mxCell")] 
    public List<MxCell>? MxCells { get; set; } 
}