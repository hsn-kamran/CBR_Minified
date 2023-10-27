using System.Xml.Serialization;

namespace CBR_Minified.Models;


[XmlRoot(ElementName = "Valute")]
public class Valute
{

    [XmlElement(ElementName = "NumCode")]
    public string NumCode { get; set; }

    [XmlElement(ElementName = "CharCode")]
    public string CharCode { get; set; }

    [XmlElement(ElementName = "Nominal")]
    public int Nominal { get; set; }

    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "Value")]
    public string Value { get; set; }

    [XmlElement(ElementName = "VunitRate")]
    public string VunitRate { get; set; }
 
    [XmlAttribute(AttributeName = "ID")]
    public string Id { get; set; }
}