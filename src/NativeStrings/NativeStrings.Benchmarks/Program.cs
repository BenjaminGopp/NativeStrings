using System;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Running;

namespace NativeStrings.Benchmarks
{
  class Program
  {
    static void Main(string[] args)
    {
      //var c = new TextConverter();
      //c.ConvertText("Bibel.txt");
      BenchmarkRunner.Run<ReadBenchmarks>();
      //BenchmarkRunner.Run<WriteBenchmarks>();

      Console.WriteLine();
    }


  }
}
