using System.Xml.Linq;

namespace Softhouse.Data
{
  public class Phone : IXmlElement
  {
    public string Number { get; private set; }
    public PhoneType Type { get; private set; }

    public Phone(string number, PhoneType type)
    {
      this.Number = number;
      this.Type = type;
    }

    public XElement ToXml()
    {
      switch (this.Type)
      {
        case PhoneType.Mobile:
          return XElement.Parse($"<mobile>{Number}</mobile>");
        case PhoneType.Landline:
          return XElement.Parse($"<landline>{Number}</landline>");
        default:
          return XElement.Parse($"<unknown>{Number}</unknown>"); // Should never happen
      }
    }
  }
}
