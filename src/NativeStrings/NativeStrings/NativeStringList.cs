using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace NativeStrings.Benchmarks
{
  public class NativeStringList : IEnumerable<NativeString>
  {
    public NativeStringList()
    {

    }

    private IntPtr _memory = IntPtr.Zero;
    private int _memoryLength = 0;
    public void ReadFromStream(Stream stream)
    {
      _memoryLength = (int)stream.Length;
      _memory = Marshal.AllocHGlobal(_memoryLength);
      unsafe
      {
        var bytes = new Span<byte>(_memory.ToPointer(), _memoryLength);
        stream.Read(bytes);
        CreateStrings();
      }

    }

    private List<NativeString> _strings = new List<NativeString>();

    public int Count => _strings.Count;

    public NativeString this[int index] => _strings[index];

    private unsafe void CreateStrings()
    {

      char* c = (char*)_memory.ToPointer();
      int start = 0;
      int length = 0;

      for (int i = 0; i < _memoryLength; c++, i+= sizeof(char))
      {

        char cc = *c;
        if (cc == '\0')
        {
          var address = _memory + start * sizeof(char);
          var nativeString = new NativeString(address, length);
          _strings.Add(nativeString);

          start += length + 1;
          length = 0;
          i += sizeof(char);
          continue;
        }

        length++;

      }

    }


    public void Free()
    {
      _strings.Clear();
      Marshal.FreeHGlobal(_memory);
    }

    public IEnumerator<NativeString> GetEnumerator()
    {
      return _strings.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}