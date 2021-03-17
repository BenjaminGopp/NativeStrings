// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


namespace InfoZoom.Kernel
{
    [Serializable]
    public class StringValueDictionary<TValue> : MyDictionary<StringValue, TValue>
    {

        public StringValueDictionary() : base(StringValueEqualityComparer.Default)
        {

        }
        public StringValueDictionary(int capacity) : base(capacity, StringValueEqualityComparer.Default)
        {
            
        }


    }



}
