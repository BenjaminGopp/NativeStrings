using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

namespace InfoZoom.Kernel
{

    public sealed class StringValueList : IDisposable, IEnumerable<StringValue>
    {

        public StringValueList(int charCapacity = 10_000)
        {
            _memory = Marshal.AllocHGlobal(charCapacity * 2);
            _nextPtr = _memory;
            CharCapacity = charCapacity;
            IsReadOnly = false;
            _items = new List<StringValue>();
        }

        public StringValueList(IntPtr memory, int size, int stringCount)
        {
            _memory = memory;
            _nextPtr = _memory;
            CharCapacity = size;
            UsedChars = size;
            IsReadOnly = true;
            _items = CreateItems(stringCount);
        }


        //copy constructor
        public StringValueList(IntPtr memory, int size, int stringCount, List<StringValue> items)
        {
            _memory = memory;
            _nextPtr = _memory;
            CharCapacity = size;
            UsedChars = size;
            IsReadOnly = true;
            _items = items;
        }


        //private unsafe Dictionary<int, int> CreateIndices(int stringCount)
        //{
        //    var dict = new Dictionary<int, int>(stringCount);
        //    var chars = new ReadOnlySpan<char>(_memory.ToPointer(), CharCapacity);

        //    int elementCount = 0;
        //    int length = 0;
        //    var start = 0;

        //    for (int i = 0; i < chars.Length; i++)
        //    {
        //        var c = chars[i];

        //        if (c == '\0')
        //        {
        //            dict.Add(elementCount, start);
        //            elementCount++;
        //            start += length + 1;
        //            length = 0;
        //            continue;
        //        }

        //        length++;

        //    }


        //    return dict;
        //}



        private unsafe List<StringValue> CreateItems(int stringCount)
        {
            var dict = new List<StringValue>(stringCount);
            var chars = new ReadOnlySpan<char>(_memory.ToPointer(), CharCapacity);

            int elementCount = 0;
            int length = 0;
            var start = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                var c = chars[i];

                if (c == '\0')
                {
                    var address = _memory + start * 2;
                    var stringValue = new StringValue(address, length);
                    dict.Add(stringValue);
                    //(dict.Add(elementCount, start);
                    elementCount++;
                    start += length + 1;
                    length = 0;
                    continue;
                }

                length++;

            }


            return dict;
        }

        private readonly IntPtr _memory;
        private IntPtr _nextPtr;
        public int UsedChars { get; private set; }
        public int CharCapacity { get; }

        public bool IsReadOnly { get; set; }
        public int Count => _items.Count;

        private List<StringValue> _items;

        //private Dictionary<int, int> _items;

        public StringValue this[int index]
        {
            get
            {
                return _items[index];
                //int start = _items[index];
                //int end = index + 1 < _items.Count ? _items[index + 1] : UsedChars;
                //var address = _memory + (start * 2);
                //return new StringValue(address, end - start - 1);
            }
        }


        public StringValue Add(ReadOnlySpan<char> value)
        {
            if (IsReadOnly)
            {
                throw new Exception("read only");
            }

            if (value.IndexOf('\0') != -1)
            {
                throw new ArgumentException("string contains null terminator");
            }

            if (value.Length + UsedChars > CharCapacity)
            {
                throw new InsufficientMemoryException("");
            }

            var strPtr = _nextPtr;
            var start = UsedChars;

            ReadOnlySpan<char> source = value;
            var target = GetTargetSpan(_nextPtr, value.Length);
            source.CopyTo(target);
            _nextPtr += value.Length * 2;
            UsedChars += value.Length;

            ReadOnlySpan<char> TERMINATOR = "\0";
            TERMINATOR.CopyTo(GetTargetSpan(_nextPtr, 1));
            _nextPtr += 2;
            UsedChars += 1;


            var nativeString = new StringValue(strPtr, value.Length);
            //_items.Add(_items.Count, start);
            _items.Add(nativeString);
            return nativeString;
        }


        private unsafe Span<char> GetTargetSpan(IntPtr ptr, int length)
        {
            var target = new Span<char>(ptr.ToPointer(), length);
            return target;
        }


        public void Dispose()
        {
            Marshal.FreeHGlobal(_memory);
        }


        public IEnumerator<StringValue> GetEnumerator()
        {
            return _items.GetEnumerator();
            //foreach (var index in _items)
            //{
            //    yield return this[index];
            //}

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public unsafe StringValueList Copy()
        {
            var copy = Marshal.AllocHGlobal(UsedChars * sizeof(char));
            var sourceSpan = new Span<char>(_memory.ToPointer(), UsedChars);
            var targetSpan = new Span<char>(copy.ToPointer(), UsedChars);
            sourceSpan.CopyTo(targetSpan);

            return new StringValueList(copy, UsedChars, _items.Count, _items.ToList());
        }

        public static StringValueList FromList(List<string> list)
        {
            var stringValueSerializer = new StringValueSerializer();
            (var memory, var size, var count) = stringValueSerializer.Serialize(list);
            return new StringValueList(memory, size, count);
        }
    }
}


