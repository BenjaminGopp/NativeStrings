using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace InfoZoom.Kernel
{


    public readonly struct StringValue
    {

        public StringValue(IntPtr address, int length)
        {
            Address = address;
            Length = length;
        }

        public IntPtr Address { get; }

        public int Length { get; }
        public static StringValue Empty => new StringValue(IntPtr.Zero, 0);


        public static implicit operator ReadOnlySpan<char>(StringValue native)
        {
            return native.Span();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<char> Span() => new Span<char>(Address.ToPointer(), Length);


        public override bool Equals(object? obj)
        {
            if (obj is StringValue n)
            {
                return Equals(n);
            }

            return false;
        }


        public bool Equals(StringValue other)
        {
            if (Length != other.Length)
                return false;

            return GetHashCode() == other.GetHashCode();
        }


        public override int GetHashCode()
        {
            return string.GetHashCode(Span());
        }


        internal int GetDeterministicHashCode()
        {
            var str = Span();
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        // Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
        // or are otherwise mitigated


        internal unsafe int GetNonRandomizedHashCode()
        {
            var src = (char*)this.Address.ToPointer();

            //fixed (char* src = &_firstChar)
            //{
                //Debug.Assert(src[this.Length] == '\0', "src[this.Length] == '\\0'");
                //Debug.Assert(((int)src) % 4 == 0, "Managed string should start at 4 bytes boundary");

                uint hash1 = (5381 << 16) + 5381;
                uint hash2 = hash1;

                uint* ptr = (uint*)src;
                int length = this.Length;

                while (length > 2)
                {
                    length -= 4;
                    // Where length is 4n-1 (e.g. 3,7,11,15,19) this additionally consumes the null terminator
                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                    hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[1];
                    ptr += 2;
                }

                if (length > 0)
                {
                    // Where length is 4n-3 (e.g. 1,5,9,13,17) this additionally consumes the null terminator
                    hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[0];
                }

                return (int)(hash1 + (hash2 * 1566083941));
            //}
        }

        public static bool operator ==(StringValue left, StringValue right)
        {
            return left.Equals(right);
        }


        public static bool operator !=(StringValue left, StringValue right)
        {
            return !(left == right);
        }


        public override string ToString()
        {
            return Span().ToString();
        }

    }

}
