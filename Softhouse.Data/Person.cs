using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Xml.Linq;

namespace Softhouse.Data
{
  public class Person : IXmlElement
  {
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Address Address { get; set; }
    public Phones PhoneNumbers { get; private set; }
    public List<FamilyMember> FamilyMembers { get; private set; }

    public Person(string firstName, string lastName)
    {
      this.FirstName = firstName;
      this.LastName = lastName;
      PhoneNumbers = new Phones();
      FamilyMembers = new List<FamilyMember>();
    }

    public XElement ToXml()
    {
      XElement xml = new XElement("person",
        new XElement("firstname", this.FirstName),
        new XElement("lastname", this.LastName)
      );
      if (Address != null)
      {
        xml.Add(Address.ToXml());
      }
      if (PhoneNumbers != null && PhoneNumbers.Count > 0)
      {
        xml.Add(PhoneNumbers.ToXml());
      }
      if (FamilyMembers != null && FamilyMembers.Count > 0)
      {
        foreach(var familyMember in FamilyMembers)
        {
          xml.Add(familyMember.ToXml());
        }
      }
      return xml;
    }
  }
}
