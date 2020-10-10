using Softhouse.Data;
using System;
using System.Linq;
using System.Text;

namespace Softhouse.Integration
{
  public class Parser
  {
    public StringBuilder Errors { get; private set; }
    public People People { get; private set; }

    private string[] lines;

    public Parser(string sourceText)
    {
      lines = sourceText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
    }

    public void Parse()
    {
      People = new People();
      Errors = new StringBuilder();
      ParsePerson(0);
    }

    private void ParsePerson(int lineNumber)
    {
      if (lineNumber < lines.Length)
      {
        var line = lines[lineNumber];
        var elements = line.Split('|');
        if (elements[0].Trim().ToUpper() != "P")
        {
          Errors.AppendLine($"Unknown element: {elements[0].Trim()}, expected P");
          ParsePerson(lineNumber + 1);
        }
        else if (elements.Length != 3)
        {
          Errors.AppendLine($"Invalid line format: {line}");
          ParsePerson(lineNumber + 1);
        }
        else
        {
          Person p = new Person(elements[1].Trim(), elements[2].Trim());
          lineNumber++;
          ParsePersonInfo(p, ref lineNumber);
          People.Add(p);
          ParsePerson(lineNumber);
        }
      }
    }

    private void ParsePersonInfo(Person person, ref int lineNumber)
    {
      if (lineNumber < lines.Length)
      {
        var line = lines[lineNumber];
        var elements = line.Split('|');
        switch (elements[0].Trim().ToUpper())
        {
          case "P":
            return;
          case "T":
            Phones phoneNumbers = ParsePhoneNumbers(elements);
            if (phoneNumbers != null)
              person.PhoneNumbers.AddRange(phoneNumbers);
            else
              Errors.AppendLine($"Invalid Phone number line {line}");
            break;
          case "A":
            Address address = ParseAddress(elements);
            if (address != null)
              person.Address = address;
            else
              Errors.AppendLine($"Invalid address format: {line}");
            break;
          case "F":
            if (elements.Length == 3)
            {
              FamilyMember familyMember = new FamilyMember(elements[1].Trim(), elements[2].Trim());
              lineNumber++;
              ParseFamilyMember(familyMember, ref lineNumber);
              person.FamilyMembers.Add(familyMember);
            }
            else
            {
              Errors.AppendLine($"Invalid Family Member line {line}");
              Errors.AppendLine("**** Parsing interrupted ****");
              string errors = Errors.ToString();
              Errors.Clear();
              throw new ParsingInterruptedException(message: errors);
            }
            break;
          default:
            Errors.AppendLine($"Invalid line {line}");
            break;
        }
        lineNumber++;
        ParsePersonInfo(person, ref lineNumber);
      }
    }

    private void ParseFamilyMember(FamilyMember person, ref int lineNumber)
    {
      if (lineNumber < lines.Length)
      {
        var line = lines[lineNumber];
        var elements = line.Split('|');
        switch (elements[0].Trim().ToUpper())
        {
          case "P": case "F":
            return;
          case "A":
            Address address = ParseAddress(elements);
            if (address != null)
              person.Address = address;
            else
              Errors.AppendLine($"Invalid address format: {line}");
            break;
          case "T":
            Phones phoneNumbers = ParsePhoneNumbers(elements);
            if (phoneNumbers != null)
              person.PhoneNumbers.AddRange(phoneNumbers);
            else
              Errors.AppendLine($"Invalid Phone number line {line}");
            break;
          default:
            Errors.AppendLine($"Invalid line {line}");
            break;
        }
      }
    }

    private Phones ParsePhoneNumbers(string[] elements)
    {
      Phones phoneList = null;
      if (elements.Length > 1 && elements.Length < 4)
      {
        phoneList = new Phones();
        for (int i = 1; i < elements.Length; i++)
        {
          string number = elements[i].Trim();
          if (!string.IsNullOrEmpty(number))
          {
            phoneList.Add(new Phone(number, i == 1 ? PhoneType.Mobile : PhoneType.Landline));
          }
        }
      }
      return phoneList;
    }

    private Address ParseAddress(string[] elements)
    {
      Address address = null;
      var elems = elements.ToList();
      if (elems.Count > 1)
      {
        while (elems.Count < 4)
          elems.Add("");
          
        address = new Address(elems[1].Trim(), elems[2].Trim(), elems[3].Trim());
      }
      return address;
    }
  }
}
