using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InfoZoom.Kernel
{

    public sealed class StringValueSerializer
    {

        public (IntPtr, int, int) Serialize(IList<string> list)
        {


            var size = GetSize(list);

            var _ptr = Marshal.AllocHGlobal((size + list.Count) * sizeof(char));
            var _nextPtr = _ptr;


            foreach (var value in list)
            {

                ReadOnlySpan<char> source = value;
                var target = GetTargetSpan(_nextPtr, value.Length);
                source.CopyTo(target);
                _nextPtr += value.Length * 2;

                ReadOnlySpan<char> NULL = "\0";
                NULL.CopyTo(GetTargetSpan(_nextPtr, 1));
                _nextPtr += 2;
            }

            return (_ptr, size + list.Count, list.Count);
        }

        public unsafe List<StringValue> Deserialize(IntPtr memory, int size)
        {
            var list = new List<StringValue>();

            var span = new Span<char>(memory.ToPointer(), size / 2);

            IntPtr nextPtr = memory;
            int length = 0;

            for (int i = 0; i < span.Length; i++)
            {
                var c = span[i];

                if (c == '\0')
                {
                    list.Add(new StringValue(nextPtr, length));
                    nextPtr += 2;
                    length = 0;
                }
                else
                {
                    nextPtr += 2;
                    length++;
                }
            }

            return list;
        }

        private static int GetSize(IList<string> list)
        {
            int size = 0;
            foreach (var x in list) size += x.Length;
            return size;
        }

        private unsafe Span<char> GetTargetSpan(IntPtr ptr, int length)
        {
            var target = new Span<char>(ptr.ToPointer(), length);
            return target;
        }


    }
}
