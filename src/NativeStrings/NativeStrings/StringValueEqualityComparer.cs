using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InfoZoom.Kernel
{

    

    public class StringValueEqualityComparer : IEqualityComparer<StringValue>
    {

        
        public static IEqualityComparer<StringValue> Default = new StringValueEqualityComparer();

        private StringValueEqualityComparer()
        {

        }

        public bool Equals(StringValue x, StringValue y)
        {
            if (x.Length != y.Length)
                return false;

            return x.GetNonRandomizedHashCode() == y.GetNonRandomizedHashCode();
        }

        public int GetHashCode(StringValue value)
        {
            return value.GetNonRandomizedHashCode();
        }


    }
}
