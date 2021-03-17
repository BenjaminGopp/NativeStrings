using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using BenchmarkDotNet.Attributes;

namespace NativeStrings.Benchmarks
{

  [PlainExporter]
  public class WriteBenchmarks
  {
    private TextConverter _textConverter = new TextConverter();

    public WriteBenchmarks()
    {
      _strings = _textConverter.ReadText("Werther.txt");
    }

    private List<string> _strings;

    [Benchmark]
    public void WriteJson()
    {
      _textConverter.CovertTextToJson(_strings);
    }

    [Benchmark]
    public void WriteBinary()
    {
      _textConverter.ConvertTextToChars(_strings);
    }

  }
}