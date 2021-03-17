using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace NativeStrings.Benchmarks
{
  internal class TextConverter
  {
    private string _fileName;

    public void ConvertText(string fileName)
    {
      _fileName = fileName;
      var readText = ReadText(fileName);
      CovertTextToJson(readText);
      ConvertTextToChars(readText);
    }

    public List<string> ReadText(string fileName)
    {
      var strings = new List<string>();
      var reader = new StreamReader(fileName);

      while (!reader.EndOfStream)
      {

        var line = reader.ReadLine();
        if (line == null)
          break;


        foreach (var word in line.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        {
          strings.Add(word);
        }

      }

      reader.Close();

      return strings;
    }

    public void ConvertTextToChars(List<string> strings)
    {
      var file = File.OpenWrite(_fileName + ".bin");
      var writer = new StreamWriter(file,Encoding.Unicode);
      foreach (var value in strings)
      {
        writer.Write(value);
        writer.Write('\0');
      }
      writer.Flush();
      writer.Close();
    }

    public void CovertTextToJson(List<string> strings)
    {
      var json = JsonConvert.SerializeObject(strings);
      File.WriteAllText(_fileName + ".json", json);
    }
  }
}