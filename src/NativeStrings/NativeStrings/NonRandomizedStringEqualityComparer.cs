// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
  // NonRandomizedStringEqualityComparer is the comparer used by default with the Dictionary<string,...> 
  // We use NonRandomizedStringEqualityComparer as default comparer as it doesnt use the randomized string hashing which 
  // keeps the performance not affected till we hit collision threshold and then we switch to the comparer which is using 
  // randomized string hashing.
  [Serializable] // Required for compatibility with .NET Core 2.0 as we exposed the NonRandomizedStringEqualityComparer inside the serialization blob
                 // Needs to be public to support binary serialization compatibility
  internal sealed class NonRandomizedStringEqualityComparer : EqualityComparer<string?>, ISerializable
  {
    internal static new IEqualityComparer<string?> Default { get; } = new NonRandomizedStringEqualityComparer();

    private NonRandomizedStringEqualityComparer() { }

    // This is used by the serialization engine.
    private NonRandomizedStringEqualityComparer(SerializationInfo information, StreamingContext context) { }

    public sealed override bool Equals(string? x, string? y) => string.Equals(x, y);

    public sealed override int GetHashCode(string? obj) => GetNonRandomizedHashCode(obj);

    // Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    // or are otherwise mitigated
    internal unsafe int GetNonRandomizedHashCode(string? str)
    {
      if (str == null)
        return 0;

      fixed (char* src = str)
      {
        //Debug.Assert(src[this.Length] == '\0', "src[this.Length] == '\\0'");
        //Debug.Assert(((int)src) % 4 == 0, "Managed string should start at 4 bytes boundary");

        uint hash1 = (5381 << 16) + 5381;
        uint hash2 = hash1;

        uint* ptr = (uint*)src;
        int length = str.Length;

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
      }
    }
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      // We are doing this to stay compatible with .NET Framework.
      //info.SetType(typeof(GenericEqualityComparer<string>));
    }

  }
}
