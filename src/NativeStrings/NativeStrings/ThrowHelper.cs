// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace System.Collections.Generic
{ 
    internal class ThrowHelper
  {
    internal static void ThrowKeyNotFoundException(object key)
    {
      throw new Exception();
    }

    internal static void ThrowInvalidOperationException_ConcurrentOperationsNotSupported()
    {
      throw new Exception();
    }

    internal static void ThrowArgumentNullException(object key)
    {
      throw new Exception();
    }

    internal static void ThrowAddingDuplicateWithKeyArgumentException(object key)
    {
      throw new Exception();
    }

    internal static void ThrowArgumentOutOfRangeException(object capacity)
    {
      throw new Exception();
    }

    internal static void ThrowArgumentOutOfRangeException()
    {
      throw new Exception();
    }

    internal static void ThrowArgumentNullException()
    {
      throw new Exception();
    }

    public static void ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen()
    {
      throw new Exception();
    }

    public static void ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion()
    {
      throw new Exception();
    }

    public static void ThrowIndexArgumentOutOfRange_NeedNonNegNumException()
    {
      throw new Exception();
    }

    public static void ThrowArgumentException()
    {
      throw new Exception();
    }

    public static void ThrowNotSupportedException()
    {
      throw new Exception();
    }

    public static void ThrowArgumentException_Argument_InvalidArrayType()
    {
      throw new Exception();
    }

    internal static void ThrowAddingDuplicateWithKeyArgumentException()
    {
      throw new Exception();
    }

    public static void IfNullAndNullsAreIllegalThenThrow<TValue>(object? value, object o)
    {
      throw new NotImplementedException();
    }

    public static void ThrowWrongValueTypeArgumentException(object? value, Type type)
    {
      throw new NotImplementedException();
    }

    public static void ThrowWrongKeyTypeArgumentException(object key, Type type)
    {
      throw new NotImplementedException();
    }

    internal static void IfNullAndNullsAreIllegalThenThrow<T>(object value)
    {
      throw new NotImplementedException();
    }
  }
}
