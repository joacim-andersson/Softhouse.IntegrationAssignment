using System.Collections.Generic;
using System.Xml.Linq;

namespace Softhouse.Data
{
  public class Phones : List<Phone>, IXmlElement
  {
    public XElement ToXml()
    {
      XElement xml = new XElement("phone");
      foreach(var phone in this)
      {
        xml.Add(phone.ToXml());
      }
      return xml;
    }
  }
}
