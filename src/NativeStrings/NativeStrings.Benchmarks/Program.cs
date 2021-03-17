using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Running;

namespace NativeStrings.Benchmarks
{
  class Program
  {
    static void Main(string[] args)
    {
      var c = new TextConverter();
      c.ConvertText("Bibel.txt");
      //BenchmarkRunner.Run<ReadBenchmarks>();
      //BenchmarkRunner.Run<WriteBenchmarks>();

      var reader = File.OpenRead("Bibel.txt.bin");

      var strings = new NativeStringList();
      strings.ReadFromStream(reader);


      foreach (var value in strings.Take(2000))
      {
        Console.WriteLine(value);
      }

      Console.ReadLine();
    }


  }
}
