using Softhouse.Integration;
using System;
using System.IO;

namespace Softhouse.IntegrationAssignment
{
  public class Program
  {
    static void Main(string[] args)
    {
      Program program = new Program();
      /*
       * You can comment out the line program.Run() and
       * uncomment the program.QuickTest() line if you
       * just want the XML written out to the screen for 
       * some hard coded data.
       */
      program.Run(args);
      //program.QuickTest();
    }

    public void Run(string[] args)
    {
      string source, output;
      if (args.Length == 0)
      {
        ShowHelp();
      }
      else
      {
        source = args[0];
        if (!File.Exists(source))
        {
          Console.Error.WriteLine($"Invalid file name: {source}");
          ShowHelp();
          return;
        }
        if (args.Length >= 2)
        {
          output = args[1];
          string outputPath, outputFileName;
          try
          {
            outputPath = Path.GetFullPath(output);
            if (!Directory.Exists(outputPath))
            {
              Console.Error.WriteLine($"Invalid file path: {outputPath}");
              ShowHelp();
              return;
            }
            outputFileName = Path.GetFileName(output);
            output = Path.Combine(outputPath, outputFileName);
          }
          catch (Exception ex)
          {
            Console.Error.WriteLine(ex.Message);
            return;
          }
          ParseFile(source, output);
        }
        else
        {
          string xml = ParseFile(source);
          if (xml != null)
            Console.WriteLine(xml);
        }
      }
    }
    
    public void QuickTest()
    {
      string content =
@"P|Carl Gustaf|Bernadotte
T | 0768 - 101801 | 08 - 101801
A | Drottningholms slott | Stockholm | 10001
F | Victoria | 1977
A | Haga Slott | Stockholm | 10002
F | Carl Philip | 1979
T | 0768 - 101802 | 08 - 101802
P | Barack | Obama
A | 1600 Pennsylvania Avenue | Washington, D.C";
      Parser p = new Parser(content);
      try
      {
        p.Parse();
      }
      catch (ParsingInterruptedException ex)
      {
        Console.Error.WriteLine("FATAL ERROR\n===========\n");
        Console.Error.WriteLine(ex.Message);
        Console.WriteLine("\nPress Enter to Exit.");
        Console.ReadLine();
        return;
      }
      string parsingErrors = p.Errors.ToString();
      if (!string.IsNullOrEmpty(parsingErrors))
      {
        Console.Error.WriteLine("Errors\n======\n");
        Console.Error.WriteLine(parsingErrors);
        Console.WriteLine();
      }
      Console.WriteLine(p.People.ToXml().ToString());
      Console.WriteLine("\nPress Enter to Exit.");
      Console.ReadLine();
    }

    private void ParseFile(string source, string outputFile)
    {
      string xml = ParseFile(source);
      if (xml != null)
      {
        try
        {
          File.WriteAllText(outputFile, xml);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }

    private string ParseFile(string source)
    {
      try
      {
        Parser parser = new Parser(File.ReadAllText(source));
        if (parser.Errors.Length > 0)
        {
          Console.Error.WriteLine("Errors\n======\n");
          Console.Error.WriteLine(parser.Errors.ToString());
          Console.Error.WriteLine();
        }
        return parser.People.ToString();
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(ex.Message);
        return null;
      }
      
    }
   
    private void ShowHelp()
    {
      Console.WriteLine(@"Usage:
programFile.exe path/sourceFile.txt [path/outputFile.xml]

If the second argument is left empty the resulting XML will be written to the console window.");
    }

  }
}
