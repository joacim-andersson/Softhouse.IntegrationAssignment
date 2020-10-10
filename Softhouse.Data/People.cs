using System.Collections.Generic;
using System.Xml.Linq;

namespace Softhouse.Data
{
  public class People : List<Person>, IXmlElement
  {
    public XElement ToXml()
    {
      XElement xml = new XElement("people");
      foreach(var person in this)
      {
        xml.Add(person.ToXml());
      }
      return xml;
    }
  }
}
