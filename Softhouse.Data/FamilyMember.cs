using System;
using System.Xml.Linq;

namespace Softhouse.Data
{
  public class FamilyMember : IXmlElement
  {
    public string Name { get; private set; }
    public string BirthYear { get; private set; }
    public Address Address { get; set; }
    public Phones PhoneNumbers { get; private set; }

    public FamilyMember(string name, string birthYear)
    {
      this.Name = name;
      this.BirthYear = birthYear;
      this.PhoneNumbers = new Phones();
    }

    public XElement ToXml()
    {
      var xml = new XElement("family",
        new XElement("name", Name),
        new XElement("born", BirthYear)
      );
      if (Address != null)
      {
        xml.Add(Address.ToXml());
      }
      if (PhoneNumbers != null && PhoneNumbers.Count > 0)
      {
        xml.Add(PhoneNumbers.ToXml());
      }
      return xml;
    }
  }
}
