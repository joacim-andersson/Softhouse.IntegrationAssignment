using System.Xml;
using System.Xml.Linq;

namespace Softhouse.Data
{
  public class Address : IXmlElement
  {
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Zip { get; private set; }

    public Address(string street, string city, string zip)
    {
      this.Street = street;
      this.City = city;
      this.Zip = zip;
    }

    public XElement ToXml()
    {
      return new XElement("address", 
        new XElement("street", this.Street),
        new XElement("city", this.City),
        new XElement("zip", this.Zip)
      );
    }
  }
}
