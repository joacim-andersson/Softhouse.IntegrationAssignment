using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Softhouse.Data
{
  public interface IXmlElement
  {
    XElement ToXml();
  }
}
