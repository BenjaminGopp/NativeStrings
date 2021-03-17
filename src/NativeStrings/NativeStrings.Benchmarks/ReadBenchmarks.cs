using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;

namespace NativeStrings.Benchmarks
{

  [PlainExporter]
  public class ReadBenchmarks
  {

    private TextConverter _textConverter = new TextConverter();

    public ReadBenchmarks()
    {
      //var _strings = _textConverter.ReadText("Werther.txt");
      //_textConverter.CovertTextToJson(_strings);
      //_textConverter.ConvertTextToChars(_strings);
    }

    [Benchmark]
    public void ReadJson()
    {
      var json = File.ReadAllText("Bibel.txt.json");
      var strings = JsonConvert.DeserializeObject<List<string>>(json);
    }

    [Benchmark]
    public void ReadBinary()
    {
      var reader = File.OpenRead("Bibel.txt.bin");

      var strings = new NativeStringList();
      strings.ReadFromStream(reader);

    }

    [Benchmark]
    public void ReadNaiveBinary()
    {
      var reader = new StreamReader("Bibel.txt.bin");

      var _indices = new List<int>();

      int start = 0;
      int length = 0;

      var c = new char[1];

      int i = 0;
      while (! reader.EndOfStream)
      {
        i = reader.Read(c);
        if (c[0] == '\0')
        {
          _indices.Add(start);
          start = i + 1;
          length = 0;
        }

        length++;
      }

    }


  }
}