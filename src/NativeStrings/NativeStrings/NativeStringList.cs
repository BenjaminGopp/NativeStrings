using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace NativeStrings.Benchmarks
{
  public class NativeStringList
  {
    public NativeStringList()
    {

    }

    private IntPtr _memory = IntPtr.Zero;

    public void ReadFromStream(Stream stream)
    {
      _memory =  Marshal.AllocHGlobal((int)stream.Length);
      unsafe
      {
        var bytes = new Span<byte>(_memory.ToPointer(), (int)stream.Length);
        stream.Read(bytes);
        ReadFromBytes(bytes);
      }

    }

    public void ReadFromBytes(ReadOnlySpan<byte> bytes)
    {
      var chars = MemoryMarshal.Cast<byte, char>(bytes);
      CreateItems(chars);
    }

    private List<int> _indices = new List<int>();
    private void CreateItems(ReadOnlySpan<char> chars)
    {

      int start = 0;
      int length = 0;

      for (int i = 0; i < chars.Length; i++)
      {
        char c = chars[i];
        if (c == '\0')
        {
          _indices.Add(start);
          start = i + 1;
          length = 0;
        }

        length++;

      }
    }


    public void Free()
    {
      Marshal.FreeHGlobal(_memory);

    }
  }
}